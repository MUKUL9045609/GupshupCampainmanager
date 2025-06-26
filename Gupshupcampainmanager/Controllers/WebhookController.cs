using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Gupshupcampaignmanager.Models.Common;
using Gupshupcampainmanager.Repository.Interface;
using System.Text.Json.Nodes;
using NLog;
using Newtonsoft.Json;

namespace Gupshupcampaignmanager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly ICampaignRepository _campaignRepository;
        private static readonly Logger _Nlogger = LogManager.GetCurrentClassLogger();

        public WebhookController(ILogger<WebhookController> logger, ICampaignRepository campaignRepository)
        {
            _logger = logger;
            _campaignRepository = campaignRepository;
        }

        [HttpPost("message-status")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] JsonElement root)
        {

            try
            {
                string rawJson = root.GetRawText();
                _Nlogger.Info("Raw JSON Received: " + rawJson);

                string app = root.GetProperty("app").GetString();
                string phone = root.GetProperty("phone").GetString();
                long timestamp = root.GetProperty("timestamp").GetInt64();

                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                DateTime dateTime = dateTimeOffset.UtcDateTime;

                string type = root.GetProperty("type").GetString();
                var payload = root.GetProperty("payload");

                string messageId = payload.GetProperty("gsId").GetString();
                string status = payload.GetProperty("type").GetString();
                string destination = payload.GetProperty("destination").GetString();

                var innerPayload = payload.GetProperty("payload");
                string reason = "";
                if (innerPayload.TryGetProperty("reason", out JsonElement reasonElement))
                {
                    reason = reasonElement.GetString();
                }

                var request = new SmsStatusRequest
                {
                    Status = status.ToLower(),
                    MessageId = messageId,
                    PhoneNumber = phone,
                    Timestamp = dateTime,
                    FailureReason = reason
                };

                await _campaignRepository.InsertOrUpdateSmsStatusAsync(request);

                _Nlogger.Info("json: " + JsonConvert.SerializeObject(payload));
                _Nlogger.Info("Received webhook of type: {Type}", type);

                return Ok();
            }
            catch (Exception ex)
            {
                _Nlogger.Info(ex, "Error processing webhook.");
                return StatusCode(500);

            }

        }
    }
}
