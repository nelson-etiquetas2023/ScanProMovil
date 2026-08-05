using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScanProMovil.Models;
using ScanProMovil.Services.Compras;

namespace ScanProMovil.ViewModels.Compras
{
    public partial class SincroComprasViewModels : ObservableObject
    {
        [ObservableProperty]
        public OrdenCompra orden = new();

        public IComprasService Service { get; set; }

        public SincroComprasViewModels(IComprasService service, OrdenCompra orden)
        {
            this.Service = service;
            Orden = orden;
        }

        [RelayCommand]
        public async Task SendPurchaseOrderAsync() 
        {
            //completar la orden para el envio por la API.
            orden.Description="orden creada en la app movil.";
            orden.Supply_Id = 0;
            orden.Supply_Name = "supply default.";
            orden.Subtotal = 0;
            orden.Impuesto = 0;
            orden.Total = 0;
            orden.Status = 0;
            orden.Reference = "doc. soncro app movil";
            orden.Sincro = true;
            orden.Tipo_Documento = "OC";
            await Service.SendPurchaseOrder(orden);
        }
    }
}
