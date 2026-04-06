using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using ScanProMovil.Services.Products;
using ScanProMovil.ViewModels;
using ScanProMovil.Views.Products;

namespace ScanProMovil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var deployLocalApi = "http://192.168.10.13:8080";
            var builder = MauiApp.CreateBuilder();


            builder.Services.AddHttpClient("scanpro", options => {
                options.BaseAddress = new Uri(deployLocalApi);
                options.Timeout = TimeSpan.FromSeconds(15);
                options.DefaultRequestHeaders.Add("User-Agent", "MauiApp");
            });

            builder.Services.AddSingleton <IProductsService, ProductsService>();
            builder.Services.AddSingleton<ProductsViewModel>();
            builder.Services.AddSingleton<GestionProducts>();
            builder.Services.AddSingleton<MainPage>();
            
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
