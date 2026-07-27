using ScanProMovil.ViewModels;

namespace ScanProMovil.Views.Compras;

public partial class AddComprasPage : ContentPage
{
	public readonly AddComprasViewModels _vm;
	public Int32 totalrows = 0;

    public AddComprasPage(AddComprasViewModels vm)
	{
		InitializeComponent();
		_vm = vm;
		BindingContext = _vm;
	}

    private async void btn_AddProducts_Clicked(object? sender, EventArgs? e)
    {

    }

    private async void btn_RemoveProducts_Clicked(object? sender, EventArgs? e)
    {

    }

    private async void Btn_Save_Order_Clicked(object? sender, EventArgs? e)
    {

    }
}