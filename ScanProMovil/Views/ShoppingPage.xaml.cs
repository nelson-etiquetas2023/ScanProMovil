using ScanProMovil.ViewModels;


namespace ScanProMovil.Views;

public partial class ShoppingPage : ContentPage
{
    public ShoppingPage(OrderViewModel vm)
	{
		InitializeComponent();		
        BindingContext = vm;
    }
}