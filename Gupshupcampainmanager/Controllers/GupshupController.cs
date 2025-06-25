using Gupshupcampainmanager.Models;
using Gupshupcampainmanager.Repository.Interface;
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

        public GupshupController(IWebHostEnvironment webHostEnvironment, IGupshupApiService gupshupApiService, ICampaignRepository campaignRepository)
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
            ViewData["Title"] = "Bulk Upload";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(IFormFile imageFile)
        {
            try
            {
                string result = await _gupshupApiService.SendWhatsAppMessage(imageFile);

                ViewBag.ResponseMessage = result;
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
            ViewData["Title"] = "Save Campaign Details";
            var campaigns = _campaignRepository.GetCampainDetails();
            ViewBag.CampaignList = campaigns.Result.ToList();
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


                string fullImageUrl = $"{Request.Scheme}://{Request.Host}{relativeImagePath}";


                CampaignDetailsRequest request = new CampaignDetailsRequest();
                request.ImagePath = fullImageUrl;
                request.Desciption = Description;
                request.Id = 0;

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
        public async Task<IActionResult> UpdatecampaignTemplate(IFormFile imageFile, string Description, int Id)
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


                string fullImageUrl = $"{Request.Scheme}://{Request.Host}{relativeImagePath}";


                CampaignDetailsRequest request = new CampaignDetailsRequest();
                request.ImagePath = fullImageUrl;
                request.Desciption = Description;
                request.Id = Id;

                var Result = _campaignRepository.InsertCampainDetails(request);


                ViewBag.ResponseMessage = $"Record updated successfully";
                ViewBag.AlertClass = "alert-success";

                var campaigns = _campaignRepository.GetCampainDetails();
                ViewBag.CampaignList = campaigns.Result.ToList();
            }
            catch (Exception ex)
            {
                ViewBag.ResponseMessage = $"An error occurred: {ex.Message}";
                ViewBag.AlertClass = "alert-danger";
            }

            return View("SaveCampaignTemplate");
        }
        public IActionResult Edit(int id)
        {
            var campaign = _campaignRepository.GetCampainDetailsById(id);
            if (campaign == null) return NotFound();

            ViewBag.Id = campaign.Result.Id;
            ViewBag.ImagePath = campaign.Result.ImagePath;
            ViewBag.Description = campaign.Result.Desciption;

            ViewData["Title"] = "Edit campaign Details";

            var campaigns = _campaignRepository.GetCampainDetails();
            ViewBag.CampaignList = campaigns.Result.ToList();

            return View("SavecampaignTemplate");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var campaign = _campaignRepository.DeletCampainDetailsById(id);
            TempData["ResponseMessage"] = "Campaign deleted successfully!";
            return RedirectToAction("SavecampaignTemplate"); // Reload view
        }
        public IActionResult DownloadSampleFile()
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "sample.csv");
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Sample file not found.");
            }
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "text/csv", "sample.csv");
        }

        [HttpGet]
        public IActionResult ChangeCampaignStatus(int id, bool IsActive)
        {
            var campaign = _campaignRepository.DeActiveCampaign(id, IsActive);
            TempData["ResponseMessage"] = "Campaign deleted successfully!";
            return RedirectToAction("SavecampaignTemplate"); // Reload view
        }
    }
}
