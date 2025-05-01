using Microsoft.AspNetCore.Mvc;
using SalesAnalyticsApi.Services;

namespace SalesAnalyticsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly RevenueCalculationService _revenueCalculationService;

        public AnalysisController(RevenueCalculationService revenueCalculationService)
        {
            _revenueCalculationService = revenueCalculationService;
        }

        [HttpGet("total-revenue")]
        public async Task<IActionResult> GetTotalRevenue([FromQuery] DateRangeRequest request)
        {
            if (request.StartDate == null || request.EndDate == null || request.StartDate > request.EndDate)
            {
                return BadRequest("Invalid date range.");
            }

            var totalRevenue = await _revenueCalculationService.CalculateTotalRevenueAsync(request.StartDate.Value, request.EndDate.Value);

            return Ok(new { totalRevenue });
        }

        [HttpGet("revenue-by-product")]
        public async Task<IActionResult> GetRevenueByProduct([FromQuery] DateRangeRequest request)
        {
            if (request.StartDate == null || request.EndDate == null || request.StartDate > request.EndDate)
            {
                return BadRequest("Invalid date range.");
            }

            var revenueByProduct = await _revenueCalculationService.CalculateRevenueByProductAsync(request.StartDate.Value, request.EndDate.Value);

            return Ok(revenueByProduct);
        }

        [HttpGet("revenue-by-category")]
        public async Task<IActionResult> GetRevenueByCategory([FromQuery] DateRangeRequest request)
        {
            if (request.StartDate == null || request.EndDate == null || request.StartDate > request.EndDate)
            {
                return BadRequest("Invalid date range.");
            }
            var revenueByCategory = await _revenueCalculationService.CalculateRevenueByCategoryAsync(request.StartDate.Value, request.EndDate.Value);
            return Ok(revenueByCategory);
        }

        [HttpGet("revenue-by-region")]
        public async Task<IActionResult> GetRevenueByRegion([FromQuery] DateRangeRequest request)
        {
            if (request.StartDate == null || request.EndDate == null || request.StartDate > request.EndDate)
            {
                return BadRequest("Invalid date range.");
            }
            var revenueByCategory = await _revenueCalculationService.CalculateRevenueByRegionAsync(request.StartDate.Value, request.EndDate.Value);
            return Ok(revenueByCategory);
        }
    }

    public class DateRangeRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

