using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TallyBoard.App.Models;
using TallyBoard.App.ViewModels;

namespace TallyBoard.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
            DataContext ??= new MainWindowViewModel();

        // restore size and position on open
        this.Opened += (_, _) =>
        {
            var s = Settings.Load();
            if (s.Width > 0 && s.Height > 0)
            {
                Width = s.Width;
                Height = s.Height;
            }
            if (s.X >= 0 && s.Y >= 0)
            {
                Position = new PixelPoint(s.X, s.Y);
            }
        };

        // save size and position on close
        this.Closing += (_, _) =>
        {
            var s = new Settings
            {
                Width = (int)Width,
                Height = (int)Height,
                X = Position.X,
                Y = Position.Y
            };
            Settings.Save(s);
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
