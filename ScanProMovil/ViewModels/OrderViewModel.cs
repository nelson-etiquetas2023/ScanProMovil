using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScanProMovil.Models;
using ScanProMovil.Services.Orders;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace ScanProMovil.ViewModels
{
    public partial class OrderViewModel : ObservableObject
    {

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
        private ObservableCollection<OrderDetails> _productos = [];

        IOrderServices services { get; set; }

        public OrderViewModel(IOrderServices Service)
        {
            services = Service;
        }

        [RelayCommand]
        public async Task GetOrdersLocalSqlite() 
        {
            if (IsLoading) 
                return;

            try
            {
                IsLoading = true;
                loadingMessage = "Cargando ordenes...";
                Debug.WriteLine("Loading ON");

                await Task.Delay(1000);

                var result = await services.GetOrdersLocalSqliteAsync();
                _allOrders = result;

                Ordenes = new 
                    ObservableCollection<Order>(_allOrders);

            }
            catch (Exception ex)
            {
                Debug.Write("error al obtener los datos locales, error:" + ex.Message);
            }
            finally 
            {
                IsLoading = false;
                loadingMessage = string.Empty;
                Debug.WriteLine("Loading OFF");
            }
        }

        [RelayCommand]
        private async void SaveOrdersAsync() 
        {
            var toast = Toast.Make("Se guardo la orden correctamente");
        }

        private void FilterOrders(string searchText) 
        {
            if (string.IsNullOrEmpty(searchText)) 
            {
                Ordenes = new ObservableCollection<Order>(_allOrders);
                return;
            }

            var filtered = _allOrders.Where(o => o.OrderNumber.Contains(searchText,
                StringComparison.OrdinalIgnoreCase)).ToList();

            Ordenes = new ObservableCollection<Order>(filtered);

        }

        partial void OnSearchTextChanged(string value)
        {
            FilterOrders(value);
        }   




    }
}
