using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using ScanProMovil.ViewModels;
using ScanProMovil.Views.Orders;

namespace ScanProMovil.Views;

public partial class ShoppingPage : ContentPage
{
    private readonly OrderViewModel _vm;


    public ShoppingPage(OrderViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_vm.RefreshListOrders) 
        {
            await Task.Yield();
            await _vm.GetOrdersLocalSqliteCommand.ExecuteAsync(null);
        }    
    }

    private async void btnAddOrders_Clicked(object? sender, EventArgs e)
    {
        var page = MauiProgram.Services!.GetService<OrdersPage>();
        await Navigation.PushAsync(page!);
    }

    private async void btnDeleteOrder_Clicked(object? sender, EventArgs? e)
    {
        // Validar la orden seleccionada.
        if (_vm.SelectedOrder == null) 
        {
            await DisplayAlertAsync("Warning", "Tiene que seleccionar una orden de la Lista...", "Ok.");
            return;
        }

        //Confirmar el borrado de la orden.
        bool  confirm = await DisplayAlertAsync("Confirmar", 
            $"¿Esta seguro de eliminar la orden numero: {_vm.SelectedOrder.OrderNumber}","Si","No");

        //si se cancela la opcion.
        if (!confirm) 
        {
            var toast = Toast.Make("Operacion cancelada", ToastDuration.Short);
            await toast.Show();
            return;
        }

        //Ejecuta el procedimiento de Borrado.
        await Task.Delay(1000);
        await _vm.DeleteOrderLocalSqliteCommand.ExecuteAsync(null);


        //actualizar la lista de ordebes
        _vm.Ordenes.Remove(_vm.SelectedOrder);
        await _vm.GetOrdersLocalSqliteCommand.ExecuteAsync(null);
    }

    private async void btnDetailsOrders_Clicked(object? sender, EventArgs? e)
    {
       
        // Validar la orden seleccionada.
        if (_vm.SelectedOrder == null)
        {
            await DisplayAlertAsync("Warning", "Tiene que seleccionar una orden de la Lista...", "Ok.");
            return;
        }

        _vm.RefreshListOrders = false;
        await Navigation.PushAsync(new OrderDetailsPage(_vm.SelectedOrder));

    }
}