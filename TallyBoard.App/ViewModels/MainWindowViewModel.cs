using System;
using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TallyBoard.App.Models;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;

namespace TallyBoard.App.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private static readonly JsonSerializerOptions JsonOpts = new() { WriteIndented = true };

    private static string AppDir =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TallyBoard");
    private static string StatePath => Path.Combine(AppDir, "state.json");

    [ObservableProperty] private int count;
    [ObservableProperty] private string? note;
    [ObservableProperty] private string status = "Ready";

    public MainWindowViewModel()
    {
        try { LoadFromDisk(); }
        catch (Exception ex) { Status = $"Load error: {ex.Message}"; }
    }

    [RelayCommand] private void Increment() => Count++;
    [RelayCommand] private void Decrement() => Count--;

    [RelayCommand]
    private void Reset()
    {
        Count = 0;
        Note = string.Empty;
        Status = "Reset";
    }

    [RelayCommand]
    private void Save()
    {
        try
        {
            Directory.CreateDirectory(AppDir);
            var state = new TallyState { Count = Count, Note = Note, SavedAtUtc = DateTime.UtcNow };
            File.WriteAllText(StatePath, JsonSerializer.Serialize(state, JsonOpts));
            Status = $"Saved → {StatePath}";
        }
        catch (Exception ex)
        {
            Status = $"Save error: {ex.Message}";
        }
    }

    [RelayCommand]
    private void Load()
    {
        try
        {
            LoadFromDisk();
            Status = File.Exists(StatePath) ? $"Loaded ← {StatePath}" : "No saved state found";
        }
        catch (Exception ex)
        {
            Status = $"Load error: {ex.Message}";
        }
    }

    [RelayCommand]
    private void CloseWindow()
    {
        var life = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        life?.MainWindow?.Close();
    }

    private void LoadFromDisk()
    {
        if (!File.Exists(StatePath)) return;
        var json = File.ReadAllText(StatePath);
        var state = JsonSerializer.Deserialize<TallyState>(json) ?? new TallyState();
        Count = state.Count;
        Note = state.Note;
    }
}
