using System.ComponentModel.DataAnnotations;

namespace ScanProMovil.Models
{
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }
        public string Product_Name { get; set; } = String.Empty;
        public string Product_Type { get; set; } = string.Empty;
        public string Unidad { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CodeBar { get; set; } = string.Empty;
    }
}
