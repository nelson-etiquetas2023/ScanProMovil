using ScanProMovil.ViewModels;

namespace ScanProMovil.Views.Products;

public partial class GestionProducts : ContentPage
{
	public GestionProducts(ProductsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}