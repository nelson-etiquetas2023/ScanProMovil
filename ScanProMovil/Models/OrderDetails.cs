using System;
using System.Collections.Generic;
using System.Text;

namespace ScanProMovil.Models
{
    public class OrderDetails
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string productId { get; set; } = null!;
        public double Cantidad { get; set; }
        public double Costo { get; set; } = 0;
    }
}
