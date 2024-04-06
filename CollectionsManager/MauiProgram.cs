using CollectionsManager.Models;
using CollectionsManager.Services;
using Microsoft.Extensions.Logging;

namespace CollectionsManager
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder = AddDependencies(builder);

            return builder.Build();
        }

        private static MauiAppBuilder AddDependencies(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<FileService>();
            builder.Services.AddSingleton<DataService>();
            builder.Services.AddSingleton<CollectionsModel>();

            return builder;
        }
    }
}
