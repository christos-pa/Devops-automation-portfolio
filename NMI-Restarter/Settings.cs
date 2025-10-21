using System;
using System.IO;
using System.Text.Json;

namespace NmiRestarterTray;

/// <summary>
/// Loads/saves app settings from a JSON file that lives next to the EXE:
///   settings.json
/// If the file is missing or invalid, sensible defaults are used and a new file is written.
/// </summary>
public record Settings(string TerminalsRoot, string JarName, string LogDir, string JavaPath)
{
    public static string FilePath
        => Path.Combine(AppContext.BaseDirectory, "settings.json");

    /// <summary>Load settings from settings.json; create with defaults if missing/bad.</summary>
    public static Settings LoadOrCreateDefault()
    {
        // Defaults
        var def = new Settings(
            TerminalsRoot: @"C:\NMI-CC-Connectors\Terminals",
            JarName: "CC_UK_NMI.jar",
            LogDir: @"C:\Ops-Tools\NMI-Restarter\Logs",
            JavaPath: "" // empty = use javaw/java from PATH
        );

        try
        {
            if (!File.Exists(FilePath))
            {
                Save(def);              // write a fresh default file
                EnsureFolders(def);
                return def;
            }

            var json = File.ReadAllText(FilePath);
            var cfg = JsonSerializer.Deserialize<Settings>(json) ?? def;

            // Basic hygiene: ensure folders exist
            EnsureFolders(cfg);
            return cfg;
        }
        catch
        {
            // On any error, fall back to defaults (and ensure folders)
            EnsureFolders(def);
            return def;
        }
    }

    /// <summary>Persist settings back to settings.json (pretty-printed).</summary>
    public static void Save(Settings s)
    {
        var opts = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(s, opts);
        File.WriteAllText(FilePath, json);
    }

    private static void EnsureFolders(Settings s)
    {
        try { if (!string.IsNullOrWhiteSpace(s.LogDir)) Directory.CreateDirectory(s.LogDir); } catch { /* ignore */ }
        // TerminalsRoot is read-only; we don't create it
    }
}
