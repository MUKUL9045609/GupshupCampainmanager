using Gupshupcampainmanager.Models;
using Gupshupcampainmanager.Repository.Interface;
using Gupshupcampainmanager.Service;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Gupshupcampainmanager.Controllers
{
    public class GupshupController : Controller
    {

        private readonly IGupshupApiService _gupshupApiService;
        private readonly ILogger<GupshupController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICampaignRepository _campaignRepository;

        public GupshupController(IWebHostEnvironment webHostEnvironment , IGupshupApiService gupshupApiService , ICampaignRepository campaignRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _gupshupApiService = gupshupApiService;
            _campaignRepository = campaignRepository;
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

                string result = await _gupshupApiService.SendWhatsAppMessage("","","", "", "", "","",""); ;

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


        public IActionResult SaveCampaignTemplate()
        {
            ViewData["Title"] = "Save campaign Details";
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SaveCampaignTemplate(IFormFile imageFile, string Description)
        {
            ViewData["Title"] = "Save campaign Details";

            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    ViewBag.ResponseMessage = "Please upload an image.";
                    ViewBag.AlertClass = "alert-danger";
                    return View("Index");
                }

                if (imageFile.Length > 5 * 1024 * 1024)
                {
                    ViewBag.ResponseMessage = "Image size exceeds 5MB limit.";
                    ViewBag.AlertClass = "alert-danger";
                    return View("Index");
                }

                if (string.IsNullOrWhiteSpace(Description))
                {
                    ViewBag.ResponseMessage = "Description is required.";
                    ViewBag.AlertClass = "alert-danger";
                    return View("Index");
                }

          
                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ViewBag.ResponseMessage = "Only PNG or JPG images are supported.";
                    ViewBag.AlertClass = "alert-danger";
                    return View("Index");
                }

              
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

           
                string uniqueFileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

              
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

              
                string relativeImagePath = $"/uploads/{uniqueFileName}";

                CampaignDetailsRequest request = new CampaignDetailsRequest();
                request.ImagePath = filePath;
                request.Desciption = Description;

                var Result = _campaignRepository.InsertCampainDetails(request);

             
                ViewBag.ResponseMessage = $"Image saved successfully at {relativeImagePath} and message with description '{Description}' prepared for sending.";
                ViewBag.AlertClass = "alert-success";
            }
            catch (Exception ex)
            {
                ViewBag.ResponseMessage = $"An error occurred: {ex.Message}";
                ViewBag.AlertClass = "alert-danger";
            }

            return View("SaveCampaignTemplate");
        }
    }
}
