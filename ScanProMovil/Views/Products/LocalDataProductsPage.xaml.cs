using ScanProMovil.ViewModels.Products;

namespace ScanProMovil.Views.Products;

public partial class LocalDataProductsPage : ContentPage
{
    private readonly ProductsLocalViewModels _vm;
 
    public LocalDataProductsPage(ProductsLocalViewModels vm)
	{
		InitializeComponent();
		_vm = vm;
		BindingContext = _vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.GetProductLocal();
        SearchEntry.Focus();
    }

    private async void ReloadProducts(object? sender, EventArgs? e)
    {
        await _vm.GetProductLocal();
        SearchEntry.Text = "";
    }

    private async void searchEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (SearchEntry.Text == "") return;    
       
        await _vm.SearchProductsCommand.ExecuteAsync(null);
        SearchEntry.Text = "";
    }
}