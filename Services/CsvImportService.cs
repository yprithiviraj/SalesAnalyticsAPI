using CsvHelper;
using Microsoft.EntityFrameworkCore;
using SalesAnalyticsApi.Data;
using SalesAnalyticsApi.Models;
using SalesAnalyticsApi.Services;
using System.Globalization;

namespace SalesAnalysticsApi.Services
{
    public class CsvImportService : ICsvImportService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CsvImportService> _logger;
        private readonly string _csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "sales_data.csv");

        public CsvImportService(ApplicationDbContext context, ILogger<CsvImportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ImportCsvAsync()
        {
            //Batch processing for better performance
            //Set the batch size as needed
            const int batchSize = 1000;
            int currentBatchSize = 0;

            try
            {
                if (!File.Exists(_csvFilePath))
                {
                    _logger.LogError("CSV file not found at the specified location.");
                    return;
                }

                using var reader = new StreamReader(_csvFilePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                await foreach (var record in csv.GetRecordsAsync<SalesOrderCsvModel>())
                {
                    if (ValidateSalesOrder(record))
                    {
                        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == record.ProductId);
                        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == record.CustomerId);

                        if (product == null || customer == null)
                        {
                            _logger.LogWarning($"Invalid Product or Customer for OrderId: {record.OrderId}");
                            continue;
                        }

                        var existingOrder = await _context.Orders
                            .FirstOrDefaultAsync(o => o.OrderId == record.OrderId.ToString());

                        //Update the existing order
                        if (existingOrder != null)
                        {
                            existingOrder.DateOfSale = record.DateOfSale;
                            existingOrder.Region = record.Region;
                            existingOrder.QuantitySold = record.QuantitySold;
                            existingOrder.UnitPrice = record.UnitPrice;
                            existingOrder.Discount = record.Discount;
                            existingOrder.PaymentMethod = record.PaymentMethod;
                            existingOrder.ProductId = record.ProductId;
                            existingOrder.CustomerId = record.CustomerId;
                            existingOrder.Product = product;
                            existingOrder.Customer = customer;
                        }
                        //Add a new order
                        else
                        {
                            var salesOrder = new Order
                            {
                                OrderId = record.OrderId.ToString(),
                                DateOfSale = record.DateOfSale,
                                Region = record.Region,
                                QuantitySold = record.QuantitySold,
                                UnitPrice = record.UnitPrice,
                                Discount = record.Discount,
                                PaymentMethod = record.PaymentMethod,
                                ProductId = record.ProductId,
                                CustomerId = record.CustomerId,
                                Product = product,
                                Customer = customer
                            };

                            _context.Orders.Add(salesOrder);
                        }

                        currentBatchSize++;

                        // If the batch size is reached, save the changes and reset the counter
                        if (currentBatchSize >= batchSize)
                        {
                            await _context.SaveChangesAsync();
                            currentBatchSize = 0;  // Reset batch size
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Invalid data for OrderId: {record.OrderId}");
                    }
                }

                // Save any remaining records if they don't fill up the last batch
                if (currentBatchSize > 0)
                {
                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation("CSV data imported successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error importing CSV: {ex.Message}");
            }
        }



        private static bool ValidateSalesOrder(SalesOrderCsvModel record)
        {
            // We can add further business logic here to validate the imported data.
            if (record.QuantitySold <= 0 || record.UnitPrice <= 0 || record.Discount < 0)
            {
                return false;
            }
            return true;
        }
    }

    // This model maps to the CSV structure
    public class SalesOrderCsvModel
    {
        public int OrderId { get; set; }
        public string ProductId { get; set; } = null!;
        public string CustomerId { get; set; } = null!;
        public DateTime DateOfSale { get; set; }
        public string Region { get; set; } = null!;
        public int QuantitySold { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public string CustomerEmail { get; set; } = null!;
        public string CustomerAddress { get; set; } = null!;

    }
}
