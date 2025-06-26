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

        [HttpPost]
        public async Task<IActionResult> ReceiveMessageStatus()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            try
            {

                // Parse the JSON payload
                var jsonObject = System.Text.Json.JsonSerializer.Deserialize<JsonObject>(body);

                // Extract relevant fields (adjust based on Gupshup's payload structure)
                var messageId = jsonObject["messageId"]?.ToString();
                var status = jsonObject["status"]?.ToString();
                var timestamp = jsonObject["timestamp"]?.ToString();

                // Validate the message ID (e.g., check for your specific message ID)
                if (string.IsNullOrEmpty(messageId) || string.IsNullOrEmpty(status))
                {
                    return BadRequest("Invalid payload: messageId or status missing.");
                }

                // Log the status update (you can modify to store in a database)
                var logEntry = $"MessageID: {messageId}, Status: {status}, Timestamp: {timestamp ?? "N/A"}, Received: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}\n";
                await System.IO.File.AppendAllTextAsync("webhook_logs.txt", logEntry);

                // Optionally, check for your specific message ID
                if (messageId == "564aa8bd-044e-41d8-8dae-5dabe83cbf92")
                {
                    // Handle specific logic for this message ID (e.g., notify or update status)
                    await System.IO.File.AppendAllTextAsync("webhook_logs.txt", $"Specific MessageID Match Found: {messageId}\n");
                }


                SmsStatusRequest request = new SmsStatusRequest();

                request.Status = status.ToLower();
                request.MessageId = messageId;
                request.PhoneNumber = "";
                request.Timestamp = DateTime.Parse(timestamp);
                request.FailureReason = "";

                await _campaignRepository.InsertOrUpdateSmsStatusAsync(request);

                // Return 200 OK to acknowledge receipt
                return Ok(new { message = "Webhook received successfully" });




                //var messageStatus = JsonSerializer.Deserialize<SmsStatusWebhookPayload>(body);
                //if (messageStatus == null)
                //{
                //    _logger.LogWarning("Webhook payload deserialization returned null.");
                //    return BadRequest();
                //}

                //_logger.LogInformation("Received SMS status update: {@Payload}", messageStatus);



                //return Ok();
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize webhook payload.");
                return BadRequest("Invalid JSON");
            }
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
                string reason = innerPayload.GetProperty("reason").GetString();

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

                return Ok(); // Always respond 200 to avoid retries
            }
            catch (Exception ex)
            {
                _Nlogger.Info(ex, "Error processing webhook.");
                return StatusCode(500);

            }

        }
    }
}
