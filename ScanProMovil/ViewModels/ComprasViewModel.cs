using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScanProMovil.Models;
using ScanProMovil.Services.Compras;
using System.Collections.ObjectModel;

namespace ScanProMovil.ViewModels
{
    public partial class ComprasViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Order> orders = new();

        [ObservableProperty]
        private ObservableCollection<OrderDetails> orderDetails = new();

        [ObservableProperty]
        private Order selectedOrder = new();

        IComprasService Service { get; set; }
        public ComprasViewModel(IComprasService service)
        {
            this.Service = service;
        }

        [RelayCommand]
        public void GetOrders()
        {

        }

        [RelayCommand]
        public void SincroOrder()
        {

        }

        [RelayCommand]
        public void UpdateOrder()
        {

        }

        [RelayCommand]
        public void DeleteOrder()
        {
        
        }

        [RelayCommand]
        public void DeactivateOrder() 
        {
        
        }
    }
}
