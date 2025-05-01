using System.ComponentModel.DataAnnotations;

namespace SalesAnalyticsApi.Models
{
    public class Order
    {
        [Key]
        public string OrderId { get; set; } = null!;
        public DateTime DateOfSale { get; set; }
        public string Region { get; set; } = null!;
        public int QuantitySold { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public string PaymentMethod { get; set; } = null!;

        public string ProductId { get; set; } = null!;
        public Product Product { get; set; } = null!;

        public string CustomerId { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
    }
}
