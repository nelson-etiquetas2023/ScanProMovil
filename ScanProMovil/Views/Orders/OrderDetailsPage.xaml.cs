using ScanProMovil.Models;
using ScanProMovil.ViewModels;

namespace ScanProMovil.Views.Orders;

public partial class OrderDetailsPage : ContentPage
{
	public OrderDetailsPage(Order order)
	{
		InitializeComponent();
		BindingContext = new OrderDetailsViewModel(order);
	}
}