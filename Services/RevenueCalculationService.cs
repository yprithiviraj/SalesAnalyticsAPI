using Microsoft.EntityFrameworkCore;
using SalesAnalyticsApi.Data;
using SalesAnalyticsApi.Models;
using System.ComponentModel.DataAnnotations;

namespace SalesAnalyticsApi.Services
{
    public class RevenueCalculationService
    {
        private readonly ApplicationDbContext _context;

        public RevenueCalculationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalculateTotalRevenueAsync(DateTime startDate, DateTime endDate)
        {
            var totalRevenue = await _context.Orders
                .AsNoTracking() // since this is just a get call we can avoid tracking for better performance
                .Where(order => order.DateOfSale >= startDate && order.DateOfSale <= endDate)
                .SumAsync(order => order.QuantitySold * order.UnitPrice * (1 - order.Discount)); // in assumption that discount is a percentage offered

            return totalRevenue;
        }


        public async Task<List<ProductRevenue>> CalculateRevenueByProductAsync(DateTime startDate, DateTime endDate)
        {
            var revenueByProduct = await _context.Orders
                .AsNoTracking()
                .Where(order => order.DateOfSale >= startDate && order.DateOfSale <= endDate)
                .GroupBy(order => order.ProductId)
                .Select(group => new ProductRevenue
                {
                    ProductId = group.Key,
                    ProductName = group.Select(o => o.Product.Name!).FirstOrDefault() ?? string.Empty,
                    TotalRevenue = group.Sum(o => o.QuantitySold * o.UnitPrice * (1 - o.Discount)) // in assumption that discount is a percentage offered
                })
                .ToListAsync();

            return revenueByProduct;
        }


        public async Task<List<CategoryRevenue>> CalculateRevenueByCategoryAsync(DateTime startDate, DateTime endDate)
        {
            var revenueByCategory = await _context.Orders
                .AsNoTracking()
                .Where(order => order.DateOfSale >= startDate && order.DateOfSale <= endDate)
                .Select(order => new
                {
                    order.Product.Category,
                    Revenue = order.QuantitySold * order.UnitPrice * (1 - order.Discount) // fetching only necessary columns // in assumption that discount is a percentage offered
                })
                .GroupBy(o => o.Category)
                .Select(group => new CategoryRevenue
                {
                    Category = group.Key,
                    TotalRevenue = group.Sum(x => x.Revenue)
                })
                .ToListAsync();

            return revenueByCategory;
        }


        public async Task<List<RegionRevenue>> CalculateRevenueByRegionAsync(DateTime startDate, DateTime endDate)
        {
            var revenueByRegion = await _context.Orders
                .AsNoTracking()
                .Where(order => order.DateOfSale >= startDate && order.DateOfSale <= endDate)
                .Select(order => new
                {
                    order.Region,
                    Revenue = order.QuantitySold * order.UnitPrice * (1 - order.Discount) // in assumption that discount is a percentage offered
                })
                .GroupBy(order => order.Region)
                .Select(group => new RegionRevenue
                {
                    Region = group.Key,
                    TotalRevenue = group.Sum(x => x.Revenue)
                })
                .ToListAsync();

            return revenueByRegion;
        }


        public class ProductRevenue
        {
            public string ProductId { get; set; } = null!;
            public string ProductName { get; set; } = null!;
            public decimal TotalRevenue { get; set; }
        }

        public class CategoryRevenue
        {
            public string Category { get; set; } = null!;
            public decimal TotalRevenue { get; set; }
        }


        public class RegionRevenue
        {
            public string Region { get; set; } = null!;
            public decimal TotalRevenue { get; set; }
        }

    }
}

