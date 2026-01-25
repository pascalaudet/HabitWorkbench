using Microsoft.Extensions.Logging;
using Velopack;

namespace HabitWorkbench
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // ✅ MUST run early so VelopackLocator is set
            try
            {
                VelopackApp.Build()
                    .SetAutoApplyOnStartup(false) // we apply on close
                    .Run();
            }
            catch
            {
                // In Debug / not-installed runs, Velopack may not be active. Ignore.
            }

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<HabitWorkbench.Data.AppDb>();
            builder.Services.AddSingleton<HabitWorkbench.Services.UiContextService>();
            builder.Services.AddSingleton<HabitWorkbench.Services.AppUpdateService>();

            return builder.Build();
        }
    }
}
