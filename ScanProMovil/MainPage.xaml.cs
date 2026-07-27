using ScanProMovil.Views;
using ScanProMovil.Views.Compras;

namespace ScanProMovil
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public async void OnTapProducts(object? sender, TappedEventArgs e)
        {
            if (sender is Image img)
            {
                await img.FadeToAsync(0.5, 100); // baja opacidad
                await img.FadeToAsync(1, 100);   // vuelve a normal
            }
            await Navigation.PushAsync(new ProductsPage());
        }

        private async void OnTapSetting(object? sender, TappedEventArgs e)
        {
            if (sender is Image img)
            {
                await img.FadeToAsync(0.5, 100); // baja opacidad
                await img.FadeToAsync(1, 100);   // vuelve a normal
            }
            await Navigation.PushAsync(new SettingPage());
        }

        private async void OnTapOrdersModule(object? sender, TappedEventArgs? e)
        {
            if (sender is Image img)
            {
                await img.FadeToAsync(0.5, 100); // baja opacidad
                await img.FadeToAsync(1, 100);   // vuelve a normal
            }

            var  page = MauiProgram.Services!.GetService<OrdersPage>();
            await Navigation.PushAsync(page!);
        }

        private async  void OnTapApiModule(object? sender, TappedEventArgs? e)
        {
            if (sender is Image img)
            {
                await img.FadeToAsync(0.5, 100); // baja opacidad
                await img.FadeToAsync(1, 100);   // vuelve a normal
            }
            await Navigation.PushAsync(new ApiPage());
        }
        private async void SqliteModule(object? sender, TappedEventArgs? e)
        {
            if (sender is Image img)
            {
                await img.FadeToAsync(0.5, 100); // baja opacidad
                await img.FadeToAsync(1, 100);   // vuelve a normal
            }

            await Navigation.PushAsync(new SqliteCrudPage());
        }

        private async void OnTapScanQrCode(object? sender, TappedEventArgs? e)
        {
            if (sender is Image img)
            {
                await img.FadeToAsync(0.5, 100); // baja opacidad
                await img.FadeToAsync(1, 100);   // vuelve a normal
            }
            await Navigation.PushAsync(new CodeQrScan());
        }

        private async void OnTapSincroData(object? sender, TappedEventArgs? e)
        {
            if (sender is Image img)
            {
                await img.FadeToAsync(0.5, 100); // baja opacidad
                await img.FadeToAsync(1, 100);   // vuelve a normal
            }
            await Navigation.PushAsync(new SincroDocuments());

        }

        private async void OnLoginUsers(object? sender, EventArgs? e)
        {
            await DisplayAlertAsync("mensaje", "modulo de login...", "OK");
        }

        private async void OnTapShopping(object? sender, TappedEventArgs? e)
        {
            if (sender is Image img)
            {
                await img.FadeToAsync(0.5, 100); // baja opacidad
                await img.FadeToAsync(1, 100);   // vuelve a normal
            }

            var page = MauiProgram.Services!.GetService<ShoppingPage>();
            await Navigation.PushAsync(page!);
            
        }

        private async void TapGestureRecognizer_Compras(object? sender, TappedEventArgs? e)
        {
            if (sender is Image img)
            {
                await img.FadeToAsync(0.5, 100); // baja opacidad
                await img.FadeToAsync(1, 100);   // vuelve a normal
            }

            var page = MauiProgram.Services!.GetService<ComprasIndexPage>();
            await Navigation.PushAsync(page!);
        }
    }
}
