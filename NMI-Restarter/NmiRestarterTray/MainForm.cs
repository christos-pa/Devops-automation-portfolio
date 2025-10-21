using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NmiRestarterTray;

public class MainForm : Form
{
    private readonly Settings _cfg;

    // UI controls
    private readonly ComboBox cboTerminals = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 320 };
    private readonly Button btnRefresh = new() { Text = "Refresh", Width = 90 };
    private readonly Button btnStart = new() { Text = "Start", Width = 90 };
    private readonly Button btnKill = new() { Text = "Kill", Width = 90 };
    private readonly Button btnRestart = new() { Text = "Restart", Width = 90 };
    private readonly Button btnRestartAll = new() { Text = "Restart All…", Width = 110 };
    private readonly TextBox txtLog = new() { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };

    // Tray
    private readonly NotifyIcon tray = new();
    private readonly ContextMenuStrip trayMenu = new();

    public MainForm(Settings cfg)
    {
        _cfg = cfg;

        Text = "NMI Terminal Restarter";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Width = 700;
        Height = 460;
        MinimumSize = new Size(620, 420);

        Directory.CreateDirectory(_cfg.LogDir);

        // Top area: wraps to a 2nd line if needed (no horizontal scroll)
        var top = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Padding = new Padding(8),
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            AutoScroll = false
        };
        top.Controls.AddRange(new Control[] { cboTerminals, btnRefresh, btnStart, btnKill, btnRestart, btnRestartAll });

        // Log area fills the rest
        var mid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };
        txtLog.Font = new Font("Consolas", 9f);
        txtLog.Dock = DockStyle.Fill;
        mid.Controls.Add(txtLog);

        Controls.Add(mid);
        Controls.Add(top);

        // Events
        cboTerminals.DisplayMember = nameof(Terminal.Name);
        Load += (_, __) => RefreshTerminals();
        btnRefresh.Click += (_, __) => RefreshTerminals();
        btnStart.Click += async (_, __) => await StartSelectedAsync();
        btnKill.Click += async (_, __) => await KillSelectedAsync();
        btnRestart.Click += async (_, __) => await RestartSelectedAsync();
        btnRestartAll.Click += async (_, __) => await RestartAllAsync();

        // Tray setup
        tray.Icon = SystemIcons.Application;
        tray.Text = "NMI Restarter";
        tray.Visible = true;
        tray.DoubleClick += (_, __) => { Show(); WindowState = FormWindowState.Normal; Activate(); };

        trayMenu.Items.Add("Show", null, (_, __) => { Show(); WindowState = FormWindowState.Normal; Activate(); });
        trayMenu.Items.Add("Restart Selected", null, async (_, __) => await RestartSelectedAsync());
        trayMenu.Items.Add("Restart All…", null, async (_, __) => await RestartAllAsync());
        trayMenu.Items.Add(new ToolStripSeparator());
        trayMenu.Items.Add("Exit", null, (_, __) => { tray.Visible = false; Application.Exit(); });
        tray.ContextMenuStrip = trayMenu;

        // Minimize-to-tray
        Resize += (_, __) => { if (WindowState == FormWindowState.Minimized) Hide(); };
        FormClosing += (_, e) => { tray.Visible = false; };
    }

    // ───────── Terminal model & discovery ─────────
    private record Terminal(string Name, string Dir, string Jar);

    private List<Terminal> DiscoverTerminals()
    {
        var list = new List<Terminal>();
        if (!Directory.Exists(_cfg.TerminalsRoot)) return list;

        foreach (var dir in Directory.GetDirectories(_cfg.TerminalsRoot, "Terminal-*"))
        {
            var jar = Path.Combine(dir, _cfg.JarName);
            list.Add(new Terminal(Path.GetFileName(dir), dir, jar));
        }
        return list.OrderBy(t => t.Name, StringComparer.OrdinalIgnoreCase).ToList();
    }

    private void RefreshTerminals()
    {
        cboTerminals.Items.Clear();
        var terms = DiscoverTerminals();
        foreach (var t in terms) cboTerminals.Items.Add(t);
        if (cboTerminals.Items.Count > 0) cboTerminals.SelectedIndex = 0;
        Log($"Found {terms.Count} terminal(s) under {_cfg.TerminalsRoot}");
    }

    private Terminal? SelectedTerminal => cboTerminals.SelectedItem as Terminal;

    // ───────── Process helpers ─────────
    private static List<int> FindPidsByJar(string jarPath)
    {
        var pids = new List<int>();
        using var q = new ManagementObjectSearcher("SELECT ProcessId, CommandLine FROM Win32_Process");
        foreach (ManagementObject mo in q.Get())
        {
            var cmd = (mo["CommandLine"] as string) ?? string.Empty;
            if (cmd.IndexOf(jarPath, StringComparison.OrdinalIgnoreCase) >= 0)
                if (mo["ProcessId"] is uint u) pids.Add((int)u);
        }
        return pids;
    }

    private static async Task<bool> KillPidsAsync(IEnumerable<int> ids)
    {
        var ok = true;
        foreach (var id in ids)
        {
            try { Process.GetProcessById(id).Kill(true); await Task.Delay(150); }
            catch { ok = false; } // may already be gone
        }
        return ok;
    }

    private string FindJavaExecutable()
        => string.IsNullOrWhiteSpace(_cfg.JavaPath) ? "javaw.exe" : _cfg.JavaPath;

    private async Task<bool> StartJarAsync(string jarPath, string workingDir)
    {
        try
        {
            var java = FindJavaExecutable();
            var psi = new ProcessStartInfo(java, $"-jar \"{jarPath}\"")
            {
                WorkingDirectory = workingDir,
                UseShellExecute = true
            };
            Process.Start(psi);
            await Task.Delay(300);
            return true;
        }
        catch { return false; }
    }

    private static HashSet<int> AsSet(IEnumerable<int> pids) => new(pids ?? Array.Empty<int>());

    private async Task<bool> VerifyStartedAsync(Terminal t, HashSet<int> oldPids, int timeoutMs = 5000, int pollMs = 500)
    {
        var sw = Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < timeoutMs)
        {
            var newPids = FindPidsByJar(t.Jar);
            if (newPids.Any(pid => !oldPids.Contains(pid))) return true; // a new PID appeared
            await Task.Delay(pollMs);
        }
        return false;
    }

    private void SetBusy(bool busy)
    {
        Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
        var enabled = !busy;
        cboTerminals.Enabled = enabled;
        btnRefresh.Enabled = btnStart.Enabled = btnKill.Enabled = btnRestart.Enabled = btnRestartAll.Enabled = enabled;
    }

    // ───────── Actions ─────────
    private async Task RestartSelectedAsync()
    {
        var t = SelectedTerminal;
        if (t is null) { MessageBox.Show("No terminal selected."); return; }

        var msg = $"Restart {t.Name} now?\n\nThis will stop and immediately re-start its Java process.\nAvoid during live payment.";
        if (MessageBox.Show(msg, "Confirm Restart", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        SetBusy(true);
        try
        {
            var old = AsSet(FindPidsByJar(t.Jar));
            Log($"> Restarting {t.Name}");

            await KillOneAsync(t);
            var started = await StartOneAsync(t);

            var verified = started && await VerifyStartedAsync(t, old);
            Log(verified ? $"  Restarted {t.Name} (verified new PID)" : $"  WARNING: Could not verify a new PID for {t.Name}");
            Log("");
        }
        finally { SetBusy(false); }
    }

    private async Task KillSelectedAsync()
    {
        var t = SelectedTerminal;
        if (t is null) { MessageBox.Show("No terminal selected."); return; }

        var msg = $"Kill {t.Name} now?\n\nThis will terminate its Java process.\nUse only if it is stuck or being restarted.";
        if (MessageBox.Show(msg, "Confirm Kill", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

        SetBusy(true);
        try
        {
            await KillOneAsync(t);
            Log("");
        }
        finally { SetBusy(false); }
    }

    private async Task StartSelectedAsync()
    {
        var t = SelectedTerminal;
        if (t is null) { MessageBox.Show("No terminal selected."); return; }

        SetBusy(true);
        try
        {
            var old = AsSet(FindPidsByJar(t.Jar));
            var ok = await StartOneAsync(t);
            var verified = ok && await VerifyStartedAsync(t, old);
            Log(verified ? $"  Started {t.Name} (verified PID)" : $"  WARNING: Could not verify start for {t.Name}");
            Log("");
        }
        finally { SetBusy(false); }
    }

    private async Task RestartAllAsync()
    {
        var terms = DiscoverTerminals();
        if (terms.Count == 0) { MessageBox.Show("No terminals found."); return; }

        var warn = "RESTART ALL will stop and start every terminal.\nDo NOT use during trading.\n\nContinue?";
        if (MessageBox.Show(warn, "Restart All", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

        SetBusy(true);
        try
        {
            foreach (var t in terms)
            {
                var old = AsSet(FindPidsByJar(t.Jar));
                Log($"> Restarting {t.Name}");
                await KillOneAsync(t);
                var started = await StartOneAsync(t);
                var verified = started && await VerifyStartedAsync(t, old);
                Log(verified ? $"  Restarted {t.Name} (verified new PID)" : $"  WARNING: Could not verify a new PID for {t.Name}");
                Log("");
            }
        }
        finally { SetBusy(false); }

        MessageBox.Show("Restart All complete.");
    }

    private async Task<bool> KillOneAsync(Terminal t)
    {
        if (!File.Exists(t.Jar)) { Log($"  [SKIP] {t.Name}: JAR not found: {t.Jar}"); return false; }

        var pids = FindPidsByJar(t.Jar);
        if (pids.Count == 0) { Log($"  No running process for {t.Name}"); return true; }

        Log($"  Killing PIDs [{string.Join(", ", pids)}] for {t.Name}");
        var ok = await KillPidsAsync(pids);
        Log(ok ? $"  Killed {pids.Count} process(es)" : $"  Some processes could not be killed");
        return ok;
    }

    private async Task<bool> StartOneAsync(Terminal t)
    {
        if (!File.Exists(t.Jar)) { Log($"  [SKIP] {t.Name}: JAR not found: {t.Jar}"); return false; }

        var ok = await StartJarAsync(t.Jar, t.Dir);
        Log(ok ? $"  Start issued for {t.Name}" : $"  FAILED to start {t.Name}");
        return ok;
    }

    // ───────── Logging ─────────
    private string LogFile => Path.Combine(_cfg.LogDir, "restart.log");

    private void Log(string message)
    {
        var line = $"{DateTime.Now:u} | {message}";
        txtLog.AppendText(line + Environment.NewLine);
        try { File.AppendAllText(LogFile, line + Environment.NewLine, Encoding.UTF8); } catch { /* ignore IO errors */ }
    }
}
