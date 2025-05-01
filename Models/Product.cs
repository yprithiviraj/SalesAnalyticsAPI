using System.ComponentModel.DataAnnotations;

namespace SalesAnalyticsApi.Models
{
    public class Product
    {
        [Key]
        public string ProductId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
