using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;

namespace Gupshupcampaignmanager.Models.Common
{
    public class PhoneNumberConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            
            var cleanedText = text.Replace("\"", "").Trim();

           
            if (decimal.TryParse(cleanedText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalValue))
            {
           
                if (decimalValue <= long.MaxValue && decimalValue >= long.MinValue)
                {
                  
                    return (long)decimalValue;
                }
            }
            return null;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return value.ToString();
        }
    }
    
}
