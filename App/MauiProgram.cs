using Arcards.Platforms.Android;
using Arcards.Services;
using Microsoft.Extensions.Logging;

#if WINDOWS
using System.Runtime.InteropServices;
#endif

namespace Arcards
{
    public static class MauiProgram
    {

#if WINDOWS
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
#endif

        public static MauiApp CreateMauiApp()
        {
#if WINDOWS
            AllocConsole();
#endif
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddScoped<CallbackService>();
            builder.Services.AddScoped<LocalizationService>();
            builder.Services.AddScoped<NFCService>();
            builder.Services.AddScoped<IFeliCa, FeliCa>();

            return builder.Build();
        }
    }
}
