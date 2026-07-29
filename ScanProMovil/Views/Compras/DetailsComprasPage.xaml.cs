using ScanProMovil.Models;
using ScanProMovil.ViewModels;

namespace ScanProMovil.Views.Compras;

public partial class DetailsComprasPage : ContentPage
{
	private readonly DetailsComprasViewModels _vm;

	public DetailsComprasPage(OrdenCompra order)
	{
		InitializeComponent();
		_vm = new DetailsComprasViewModels(order);
		BindingContext = _vm;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _vm.IsLoading = true;
        await Task.Delay(2000); // Simulate a delay
        _vm.IsLoading = false;

    }



}