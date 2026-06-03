using System;
using System.Collections.Generic;
using System.Text;

namespace ScanProMovil.Models
{
    public class OrderDetails
    {
        public int OrderId { get; set; }
        public string OrderNumer { get; set; } = null!;
        public string productId { get; set; } = null!;
        public double Cantidad { get; set; }
    }
}
