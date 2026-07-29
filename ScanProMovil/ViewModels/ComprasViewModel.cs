using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;
using ScanProMovil.Models;
using ScanProMovil.Services.Compras;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ScanProMovil.ViewModels
{
    public partial class ComprasViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool refreshListOrders = true;

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        public bool isLoading;

        [ObservableProperty]
        private string loadingMessage = string.Empty;

        private List<OrdenCompra> _allOrders = new();

        [ObservableProperty]
        private ObservableCollection<OrdenCompra> ordenes = [];

        [ObservableProperty]
        public ObservableCollection<OrdenCompra> FilteredOrders { get; } = new();

        [ObservableProperty]
        private int totalOrders;

        [ObservableProperty]
        private OrdenCompra? selectedOrder;

        IComprasService Service;

        public ComprasViewModel(IComprasService service)
        {
            this.Service = service;
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

                var result = await Service.GetOrdersLocalSqliteAsync();
                _allOrders = result;

                Ordenes = new ObservableCollection<OrdenCompra>(_allOrders);

                TotalOrders = _allOrders.Count;

            }
            catch (SqliteException ex)
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
    }
}
