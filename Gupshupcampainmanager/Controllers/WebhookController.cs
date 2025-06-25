using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Gupshupcampaignmanager.Models.Common;
using Gupshupcampainmanager.Repository.Interface;
using System.Text.Json.Nodes;
using NLog;

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
                var jsonObject = JsonSerializer.Deserialize<JsonObject>(body);

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
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize webhook payload.");
                return BadRequest("Invalid JSON");
            }
        }



        [HttpPost("message-status")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] JsonElement payload)
        {
            try
            {
                string type = payload.GetProperty("type").GetString();
                _Nlogger.Info("json", payload);
                _Nlogger.Info("Received webhook of type: {Type}", type);

                switch (type)
                {
                    case "message":
                        HandleInboundMessage(payload);
                        break;

                    case "message-event":
                        HandleMessageEvent(payload);
                        break;

                    default:
                        _logger.LogWarning("Unhandled event type: {Type}", type);
                        break;
                }

                return Ok(); // Always respond 200 to avoid retries
            }
            catch (Exception ex)
            {
                _Nlogger.Info(ex, "Error processing webhook.");
                return StatusCode(500);
            }
        }

        private void HandleInboundMessage(JsonElement payload)
        {
            var msg = payload.GetProperty("payload");
            string messageId = msg.GetProperty("id").GetString();
            string senderPhone = msg.GetProperty("sender").GetProperty("phone").GetString();
            string messageType = msg.GetProperty("type").GetString();

            _Nlogger.Info(
                "Inbound message from {Phone}, type: {MessageType}, ID: {MessageId}",
                senderPhone, messageType, messageId
            );

            // Optional: Handle message content (text, image, etc.)
        }

        private void HandleMessageEvent(JsonElement payload)
        {
            var evt = payload.GetProperty("payload");
            string status = evt.GetProperty("type").GetString();       // e.g., delivered, read, failed
            string destination = evt.GetProperty("destination").GetString(); // Receiver phone
            string messageId = evt.GetProperty("id").GetString();

            _Nlogger.Info(
                "Message-event: {Status} for {Destination}, ID: {MessageId}",
                status, destination, messageId
            );

            // Optional: Save to DB or update status
        }
    }
}
