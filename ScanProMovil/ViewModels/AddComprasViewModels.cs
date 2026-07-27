using CommunityToolkit.Mvvm.ComponentModel;
using ScanProMovil.Models;
using System.Collections.ObjectModel;

namespace ScanProMovil.ViewModels
{
    public partial class AddComprasViewModels : ObservableObject
    {
        [ObservableProperty]
        private bool refreshListOrders = true;

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        public bool isLoading;

        [ObservableProperty]
        private string loadingMessage = string.Empty;

        private List<Order> _allOrders = new();

        [ObservableProperty]
        private ObservableCollection<Order> ordenes = [];

        [ObservableProperty]
        public ObservableCollection<Order> FilteredOrders { get; } = new();

        [ObservableProperty]
        private int totalOrders;

        [ObservableProperty]
        private Order newOrder = new();

        [ObservableProperty]
        private Order? selectedOrder;




    }
}
