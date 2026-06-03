using ScanProMovil.Models;
using ScanProMovil.ViewModels;


namespace ScanProMovil.Views;

public partial class OrdersPage : ContentPage
{
    private readonly OrderViewModel _vm;

    public OrdersPage(OrderViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
        ItemsCollection.ItemsSource = _vm.Productos;
        _vm.Productos.CollectionChanged += (s, e) => UpdateGrandTotal(); 
    }

    void UpdateGrandTotal() 
    {
        double totcantidad = _vm.Productos.Sum(x => x.Cantidad);
        Int32 totalrows = _vm.Productos.Count();
        TotalCantEntry.Text = Convert.ToString(totcantidad);
        TotalRowsEntry.Text = Convert.ToString(totalrows);
    }

    public async void btn_AddProducts_Clicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtProductIdEntry.Text.Trim())) 
        {
            await DisplayAlertAsync("validation", "Enter Product id...", "Ok");
            return;
        }

        var item = new OrderDetails()
        {
            productId = txtProductIdEntry.Text.Trim(),
            Cantidad = Convert.ToDouble(txtQuantityEntry.Text)
        };

        _vm.Productos.Add(item);
        txtProductIdEntry.Text = string.Empty;
        txtQuantityEntry.Text = string.Empty;
        txtProductIdEntry.Focus();
        UpdateGrandTotal();
    }

    private void btn_RemoveProducts_Clicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is OrderDetails item) 
        {
            _vm.Productos.Remove(item);
            UpdateGrandTotal();
            
        }

    }
}