namespace Gupshupcampaignmanager.Models.Common
{
    public class SmsStatusRequest
    {
        public string Type { get; set; }
        public string Status { get; set; }
        public string MessageId { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public string FailureReason { get; set; }
        public string RawJson { get; set; }
    }
}
