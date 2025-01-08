using Microsoft.Extensions.Logging;

namespace BudgetManager.Components
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add Blazor WebView for rendering Razor components
            builder.Services.AddMauiBlazorWebView();

            // Register DealService as a Singleton for Dependency Injection
            builder.Services.AddSingleton<Services.DealService>();

#if DEBUG
            // Enable developer tools and debug logging in debug mode
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
