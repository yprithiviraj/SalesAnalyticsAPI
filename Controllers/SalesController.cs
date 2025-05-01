using Microsoft.AspNetCore.Mvc;
using SalesAnalysticsApi.Services;

namespace SalesAnalyticsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly CsvImportService _csvImportService;

        public SalesController(CsvImportService csvImportService)
        {
            _csvImportService = csvImportService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCsv()
        {
            await _csvImportService.ImportCsvAsync();
            return Ok("CSV import initiated.");
        }
    }

    public class ImportRequest
    {
        public string? FilePath { get; set; }
    }

}

