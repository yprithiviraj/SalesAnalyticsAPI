using System.ComponentModel.DataAnnotations;

namespace SalesAnalyticsApi.Models
{
    public class Customer
    {
        [Key]
        public string CustomerId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
