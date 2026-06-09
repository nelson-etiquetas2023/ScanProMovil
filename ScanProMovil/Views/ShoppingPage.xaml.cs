using ScanProMovil.ViewModels;


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

        if (_vm.Ordenes.Count == 0) 
        {
            await Task.Yield();
            await _vm.GetOrdersLocalSqliteCommand.ExecuteAsync(null);
        }
    }
}