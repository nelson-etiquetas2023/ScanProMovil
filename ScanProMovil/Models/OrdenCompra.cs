using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ScanProMovil.Models
{
    public class OrdenCompra
    {
        [Key]
        public int OrderId { get; set; }
        public string Numero { get; set; } = null!; //PK
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = "";
        public bool Sincro { get; set; }
        public string Tipo_Documento { get; set; } = "";
        public double Subtotal { get; set; }
        public double Impuesto { get; set; }
        public double Total { get; set; }
        public int Supply_Id { get; set; }
        public string Supply_Name { get; set; } = "";
        public string Reference { get; set; } = "";
        public int Status { get; set; }
        public int ItemsNumber { get; set; } = 0;
        public ObservableCollection<DetalleCompra> Items { get; set; } = [];

        public string StatusTexto
        {
            get
            {
                return Status switch
                {
                    0 => "Pendiente",
                    1 => "Modificado",
                    2 => "Sincronizado",
                    3 => "Cerrado",
                    _ => "Desconocido"
                };
            }
        }
        public Color BorderColor =>
          Status switch
          {
              0 => Colors.OrangeRed,
              2 => Colors.GreenYellow,
              _ => Colors.Gold
          };

    }
}
