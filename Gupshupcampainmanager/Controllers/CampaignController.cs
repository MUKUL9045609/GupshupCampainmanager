using Gupshupcampainmanager.Repository.Interface;
using Gupshupcampainmanager.Service;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
namespace Gupshupcampainmanager.Controllers
{
    public class CampaignController :  Controller
    {

        private readonly IGupshupApiService _gupshupApiService;
        private readonly IConfiguration _configuration;

        public CampaignController(IGupshupApiService gupshupApiService, IConfiguration configuration)
        {
            _gupshupApiService = gupshupApiService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Createcampaign()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Createcampaign(IFormFile csvFile)
        {
            if (csvFile != null && csvFile.Length > 0)
            {
                var csvData = await _gupshupApiService.ReadCsvAsync(csvFile);
                ViewBag.ExcelData = csvData; 
            }

            return View();
        }
    }
}
