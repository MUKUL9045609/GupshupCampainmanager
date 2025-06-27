using CsvHelper.Configuration;
using Gupshupcampaignmanager.Models.Common;

namespace Gupshupcampaignmanager.Models
{
    public class WhatsAppRecipient
    {
        public long? Phone { get; set; }
        public string Name { get; set; }
    }
    public class WhatsAppRecipientMap : ClassMap<WhatsAppRecipient>
    {
        public WhatsAppRecipientMap()
        {
            Map(m => m.Name);
            Map(m => m.Phone).TypeConverter<PhoneNumberConverter>(); // Apply custom converter here
        }
    }
}
