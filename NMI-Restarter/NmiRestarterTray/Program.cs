using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace NmiRestarterTray;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        if (!IsAdministrator())
        {
            var exe = Process.GetCurrentProcess().MainModule!.FileName!;
            try { Process.Start(new ProcessStartInfo(exe) { UseShellExecute = true, Verb = "runas" }); } catch { return; }
            return;
        }

        var cfg = Settings.LoadOrCreateDefault();   // ← load settings.json next to the EXE
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm(cfg));         // ← pass settings into the form

    }


    static bool IsAdministrator()
    {
        var wi = WindowsIdentity.GetCurrent();
        var wp = new WindowsPrincipal(wi);
        return wp.IsInRole(WindowsBuiltInRole.Administrator);
    }
}
