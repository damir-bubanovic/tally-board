using System;
using System.IO;
using System.Text.Json;

namespace TallyBoard.App.Models;

public class Settings
{
    public int Width { get; set; } = 480;
    public int Height { get; set; } = 360;
    public int X { get; set; } = -1;
    public int Y { get; set; } = -1;

    private static string Dir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TallyBoard");
    private static string PathFile => System.IO.Path.Combine(Dir, "settings.json");
    private static readonly JsonSerializerOptions Opts = new() { WriteIndented = true };

    public static Settings Load()
    {
        try { return File.Exists(PathFile) ? (JsonSerializer.Deserialize<Settings>(File.ReadAllText(PathFile)) ?? new()) : new(); }
        catch { return new(); }
    }
    public static void Save(Settings s)
    {
        Directory.CreateDirectory(Dir);
        File.WriteAllText(PathFile, JsonSerializer.Serialize(s, Opts));
    }
}
