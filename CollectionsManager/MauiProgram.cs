using CollectionsManager.Models;
using CollectionsManager.Pages;
using CollectionsManager.Services;
using CollectionsManager.ViewModels;
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
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddCollectionViewModel>();
            builder.Services.AddSingleton<AddCollection>();
            builder.Services.AddTransient<EditCollectionViewModel>();
            builder.Services.AddSingleton<EditCollection>();
            builder.Services.AddSingleton<AddItem>();
            builder.Services.AddSingleton<AddItemViewModel>();
            builder.Services.AddTransient<CollectionSummaryViewModel>();
            builder.Services.AddTransient<CollectionSummary>();

            return builder;
        }
    }
}
