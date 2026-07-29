using System.Text.Json.Serialization;

namespace ScanProMovil.Models
{
    public class DetalleCompra
    {
        public int OrderId { get; set; }
        public string Numero { get; set; } = null!;
        public string Product_id { get; set; } = null!;
        public double Cantidad { get; set; }
        public double Costo { get; set; } = 0;
        public decimal Subtotal { get; set; }
        
    }
}
