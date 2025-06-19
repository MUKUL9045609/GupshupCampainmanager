using Gupshupcampainmanager.Models;
using Gupshupcampainmanager.Service;
using Microsoft.AspNetCore.Mvc;

namespace Gupshupcampainmanager.Controllers
{
    public class GupshupController : Controller
    {

        private readonly GupshupApiService _gupshupApiService;
        private readonly ILogger<GupshupController> _logger;
        private readonly IConfiguration _configuration;

        public GupshupController()
        {
        }



        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateTemplate()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateTemplate(GupshupTemplateModel model)
        {
            if (ModelState.IsValid)
            {
                //var result = await _apiService.CreateTemplate(model);
                ViewBag.Response = null;
            }
            return View();
        }

        public IActionResult SendMessage()
        {
            ViewData["Title"] = "Send WhatsApp Message";
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendMessage(IFormFile imageFile, string Description)
        {
            try
            {
                //ViewData["Title"] = "Send WhatsApp Message";

                //// Load credentials from configuration
                //string apiKey = _configuration["Gupshup:ApiKey"];
                //string partnerAppToken = _configuration["Gupshup:PartnerAppToken"];
                //string appId = _configuration["Gupshup:AppId"];
                //string source = _configuration["Gupshup:Source"];
                //string destination = _configuration["Gupshup:Destination"];
                //string appName = _configuration["Gupshup:AppName"];
                //string templateId = _configuration["Gupshup:TemplateId"];

                //// Validate credentials
                //if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(partnerAppToken) || string.IsNullOrEmpty(appId) ||
                //    string.IsNullOrEmpty(source) || string.IsNullOrEmpty(destination) || string.IsNullOrEmpty(appName) ||
                //    string.IsNullOrEmpty(templateId))
                //{
                //    _logger.LogError("One or more Gupshup configuration values are missing at {Time}", DateTime.Now);
                //    ViewBag.ResponseMessage = "Configuration error: Missing Gupshup credentials.";
                //    ViewBag.AlertClass = "alert-danger";
                //    return View();
                //}

                //if (imageFile == null || imageFile.Length == 0)
                //{
                //    ViewBag.ResponseMessage = "Please upload an image.";
                //    ViewBag.AlertClass = "alert-danger";
                //    return View();
                //}

                //if (string.IsNullOrEmpty(offerText))
                //{
                //    ViewBag.ResponseMessage = "Offer text is required.";
                //    ViewBag.AlertClass = "alert-danger";
                //    return View();
                //}

                //string imageHandleId = await _gupshupApiService.UploadImageToGupshup(partnerAppToken, appId, imageFile);


                //string result = await _gupshupApiService.SendWhatsAppMessage("", "", "", "", "", "", Description);

                ViewBag.ResponseMessage = "Message sent successfully! ";
                ViewBag.AlertClass = "alert-success";
            }
            catch (Exception ex)
            {
                ViewBag.ResponseMessage = "Error: " + ex.Message;
                ViewBag.AlertClass = "alert-danger";
            }

            return View();
        }
    }
}
