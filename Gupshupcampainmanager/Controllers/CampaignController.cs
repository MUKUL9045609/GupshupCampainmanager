using Gupshupcampainmanager.Models;
using Gupshupcampainmanager.Repository.Interface;
using Gupshupcampainmanager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using OfficeOpenXml;
using System.Diagnostics.Eventing.Reader;
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
        public async Task<IActionResult> AddEditCustomer(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                var customer = await _campaignrepo.GetCustomerDetailByIdAsync(id.Value);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }
            else
            {
                return View(new CustomerViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEditCustomer(CustomerViewModel model)
        {
            

            if (model.Id > 0)
            {
               
                await _campaignrepo.EditCustomerAsync(model);
                TempData["SuccessMessage"] = "Customer updated successfully.";
            }
            else
            {
              
                var allCustomers = await _campaignrepo.ContextListAsync(new CustomerReqeust());
                var exists = allCustomers.Any(x => x.MobileNo == model.MobileNo);

                if (exists)
                {
                    ModelState.AddModelError("MobileNo", "Mobile number already exists.");
                    return View(model);
                }

                await _campaignrepo.InsertCustomerAsync(model);
                TempData["SuccessMessage"] = "Customer added successfully.";
            }

            return RedirectToAction("CustomerList");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            try
            {
                await _campaignrepo.DeleteCustomer(id);
                return Json(new { success = true, message = "Customer deleted successfully." });
            }
            catch (Exception ex)
            {
               
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
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

       


        [HttpGet]
        public IActionResult ChangeCampaignStatus(int id, bool IsActive)
        {
            var campaign = _campaignrepo.DeActiveCampaign(id, IsActive);
            TempData["ResponseMessage"] = "Campaign deleted successfully!";
            return RedirectToAction("SavecampaignTemplate"); // Reload view
        }
    }
}
