using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using ScanProMovil.Services.Products;
using ScanProMovil.ViewModels;
using ScanProMovil.Views.Products;

namespace ScanProMovil
{
    public static class MauiProgram
    {

        // Exponemos el ServiceProvider para usarlo en cualquier parte
        public static IServiceProvider? Services { get; private set; }

        public static MauiApp CreateMauiApp()
        {
            Preferences.Set("FileServerBaseUrlImages", "https://scanapi.dpdns.org/uploads/");

            var deployLocalApi = "https://scanapi.dpdns.org:443";
            var builder = MauiApp.CreateBuilder();


            builder.Services.AddHttpClient("scanpro", options => {
                options.BaseAddress = new Uri(deployLocalApi);
                options.Timeout = TimeSpan.FromSeconds(15);
                options.DefaultRequestHeaders.Add("User-Agent", "MauiApp");
            });

            builder.Services.AddSingleton <IProductsService, ProductsService>();
            builder.Services.AddTransient<ProductsViewModel>();
            
            builder.Services.AddTransient<GestionProducts>();
            builder.Services.AddTransient<ProductDetails>();

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

            var app = builder.Build();
            // Guardamos el ServiceProvider
            Services = app.Services;

            return app;

        }
    }
}
