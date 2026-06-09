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
        public bool isLoading;

        [ObservableProperty]
        private string loadingMessage = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Order> ordenes = [];

        [ObservableProperty]
        private ObservableCollection<OrderDetails> _productos = [];

        IOrderServices services { get; set; }

        public OrderViewModel(IOrderServices Service)
        {
            services = Service;
            CreateTablesOrders();
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

                Ordenes = new 
                    ObservableCollection<Order>(await services.GetOrdersLocalSqliteAsync());
            
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

        public void LoadDataFake() 
        {
           
        }
        
        [RelayCommand]
        private async void SaveOrdersAsync() 
        {
            var toast = Toast.Make("Se guardo la orden correctamente");
        }
        
        public void CreateTablesOrders() 
        {
          
        }
        
     
    }
}
