using HabitWorkbench.Data;
using System.Text;

namespace HabitWorkbench;

public partial class StartupPage : ContentPage
{
    private readonly AppDb _db;
    private readonly IServiceProvider _services;

    public StartupPage(AppDb db, IServiceProvider services)
    {
        InitializeComponent();
        _db = db;
        _services = services;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await StartAsync();
    }

    private async Task StartAsync()
    {
        try
        {
            statusLabel.Text = "Initializing local database...";
            await _db.InitAsync();

            statusLabel.Text = "Launching UI...";
            Application.Current!.MainPage = _services.GetRequiredService<MainPage>();
        }
        catch (Exception ex)
        {
            var logPath = await WriteStartupLogAsync(ex);

            Content = new VerticalStackLayout
            {
                Padding = 24,
                Spacing = 14,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label { Text = "Couldn’t open local data store.", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center },
                    new Label { Text = "The app can’t create/open its local database on this device.", Opacity = 0.9, HorizontalTextAlignment = TextAlignment.Center },
                    new Label { Text = $"Log file:\n{logPath}", FontSize = 12, Opacity = 0.8, HorizontalTextAlignment = TextAlignment.Center },
                }
            };
        }
    }

    private static async Task<string> WriteStartupLogAsync(Exception ex)
    {
        var logDir = Path.Combine(FileSystem.AppDataDirectory, "logs");
        Directory.CreateDirectory(logDir);

        var logPath = Path.Combine(logDir, "startup.log");

        var sb = new StringBuilder();
        sb.AppendLine(DateTimeOffset.Now.ToString("u"));
        sb.AppendLine("Startup failure");
        sb.AppendLine($"AppDataDirectory: {FileSystem.AppDataDirectory}");
        sb.AppendLine(ex.ToString());

        await File.WriteAllTextAsync(logPath, sb.ToString());
        return logPath;
    }
}
