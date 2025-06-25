using Gupshupcampainmanager.Models;
using Gupshupcampainmanager.Repository.Interface;
using Gupshupcampainmanager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using OfficeOpenXml;
namespace Gupshupcampainmanager.Controllers
{
    [Authorize]
    public class CampaignController :  Controller
    {

        private readonly IGupshupApiService _gupshupApiService;
        private readonly IConfiguration _configuration;
        private readonly ICampaignRepository _campaignrepo;

        public CampaignController(IGupshupApiService gupshupApiService, IConfiguration configuration, ICampaignRepository campaignrepo)
        {
            _gupshupApiService = gupshupApiService;
            _configuration = configuration;
            _campaignrepo = campaignrepo;
        }

        [HttpGet]
        public IActionResult Createcampaign()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CustomerList()
        {
            var List =_campaignrepo.ContextListAsync(new Models.CustomerReqeust()).Result;
            var customerlist = new customerResponse
            {
                Customers = List
            };
            return View(customerlist);
        }
        [HttpPost]
        public async Task<IActionResult> Createcampaign(IFormFile csvFile)
        {
            if (csvFile != null && csvFile.Length > 0)
            {
                var csvData = await _gupshupApiService.ReadCsvAsync(csvFile);
                ViewBag.ExcelData = csvData; 
            }

            return Redirect("CustomerList");
        }

        [HttpGet]
        public IActionResult ChangeCampaignStatus(int id, bool IsActive)
        {
            var campaign = _campaignrepo.DeActiveCampaign(id, IsActive);
            TempData["ResponseMessage"] = "Campaign deleted successfully!";
            return RedirectToAction("SavecampaignTemplate"); // Reload view
        }
    }
}
