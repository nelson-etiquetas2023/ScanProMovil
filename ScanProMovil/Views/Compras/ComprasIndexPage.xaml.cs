using ScanProMovil.ViewModels;

namespace ScanProMovil.Views.Compras;

public partial class ComprasIndexPage : ContentPage
{
    private readonly ComprasViewModel _vm;
    public ComprasIndexPage(ComprasViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
        Preferences.Set("SelectMultipleRows", false);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_vm.RefreshListOrders)
        {
            await Task.Yield();
            await _vm.GetOrdersLocalSqliteCommand.ExecuteAsync(null);
        }


        bool selectMultiple = Preferences.Get("SelectMultipleRows", false);

        if (selectMultiple)
        {
            //habilitar la seleccion multiple en la lista de ordenes.
            CvOrdenes.SelectionMode = SelectionMode.Multiple;
        }
    }



    private async void btnAddOrders_Clicked(object? sender, EventArgs? e)
    {
        //Validar la orden de Compra Seleccionada.
        _vm.RefreshListOrders = true;
        var page = MauiProgram.Services!.GetService<AddComprasPage>();
        await Navigation.PushAsync(page!);
    }

    private async void btnDetailsOrders_Clicked(object? sender, EventArgs? e)
    {
        _vm.RefreshListOrders = true;
        var page = MauiProgram.Services!.GetService<DetailsComprasPage>();
        await Navigation.PushAsync(page!);
    }

    private async void btnConfig_Clicked(object? sender, EventArgs? e)
    {
        _vm.RefreshListOrders = true;
        var page = MauiProgram.Services!.GetService<ConfigComprasPage>();
        await Navigation.PushAsync(page!);
    }

    private async void btn_sincrOrdenes_Clicked(object? sender, EventArgs? e)
    {
        _vm.RefreshListOrders = true;
        var page = MauiProgram.Services!.GetService<SincroComprasPage>();
        await Navigation.PushAsync(page!);
    }

    private async void btnDeleteOrder_Clicked(object? sender, EventArgs? e)
    {

    }
}