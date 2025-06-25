using System.Text.Json.Serialization;

namespace Gupshupcampaignmanager.Models.Common
{
    public class SmsStatusWebhookPayload
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("reason")]
        public string FailureReason { get; set; }
    }
}
