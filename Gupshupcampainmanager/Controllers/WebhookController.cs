using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Gupshupcampaignmanager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(ILogger<WebhookController> logger)
        {
            _logger = logger;
        }

        [HttpPost("message-status")]
        public IActionResult ReceiveMessageStatus()
        {
            using var reader = new StreamReader(Request.Body);
            var body = reader.ReadToEndAsync().Result;
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            
            _logger.LogInformation("Received webhook data: {Data}", body);

            
            if (data.TryGetValue("type", out var type) && data.TryGetValue("status", out var status))
            {
                string messageStatus = status.ToString();
                _logger.LogInformation("Message status: {Type} - {Status}", type, messageStatus);

                switch (messageStatus.ToLower())
                {
                    case "sent":

                        break;
                    case "delivered":

                        break;
                    case "read":
                        
                        break;
                    case "failed":
                        
                        break;
                }
            }

            return Ok();
        }
    }
}
