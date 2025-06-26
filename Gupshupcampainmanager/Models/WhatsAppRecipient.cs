using CsvHelper.Configuration;
using Gupshupcampaignmanager.Models.Common;

namespace Gupshupcampaignmanager.Models
{
    public class WhatsAppRecipient
    {
        public long? phone { get; set; }
        public string Name { get; set; }
    }
    public class WhatsAppRecipientMap : ClassMap<WhatsAppRecipient>
    {
        public WhatsAppRecipientMap()
        {
            Map(m => m.Name);
            Map(m => m.phone).TypeConverter<PhoneNumberConverter>(); // Apply custom converter here
        }
    }
}
