using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Gupshupcampaignmanager.Models.Common;
using Gupshupcampainmanager.Repository.Interface;

namespace Gupshupcampaignmanager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly ICampaignRepository _campaignRepository;

        public WebhookController(ILogger<WebhookController> logger, ICampaignRepository campaignRepository)
        {
            _logger = logger;
            _campaignRepository = campaignRepository;
        }

        [HttpPost("message-status")]
        public async Task<IActionResult> ReceiveMessageStatus()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            try
            {
                var messageStatus = JsonSerializer.Deserialize<SmsStatusWebhookPayload>(body);
                if (messageStatus == null)
                {
                    _logger.LogWarning("Webhook payload deserialization returned null.");
                    return BadRequest();
                }

                _logger.LogInformation("Received SMS status update: {@Payload}", messageStatus);

                SmsStatusRequest request = new SmsStatusRequest();

                request.Status = messageStatus.Status?.ToLower();
                request.MessageId = messageStatus.MessageId;
                request.PhoneNumber = messageStatus.PhoneNumber;
                request.Timestamp = DateTime.Parse(messageStatus.Timestamp);
                request.FailureReason = messageStatus.FailureReason;

                await _campaignRepository.InsertOrUpdateSmsStatusAsync(request); 

                return Ok();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize webhook payload.");
                return BadRequest("Invalid JSON");
            }
        }
    }
}
