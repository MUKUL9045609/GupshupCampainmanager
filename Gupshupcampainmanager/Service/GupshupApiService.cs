using Gupshupcampainmanager.Models;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using Gupshupcampainmanager.Helpers;
using Gupshupcampainmanager.Repository.Interface;
using System.Globalization;
using CsvHelper;
using Gupshupcampaignmanager.Models;

namespace Gupshupcampainmanager.Service
{
    public class GupshupApiService : IGupshupApiService
    {
        private static readonly HttpClient client = new HttpClient();

        private IConfiguration _configuration { get; }
        private readonly ICampaignRepository _campaignRepository;


        public GupshupApiService(IConfiguration configuration, ICampaignRepository campaignRepository)
        {
            _configuration = configuration;
            _campaignRepository = campaignRepository;
        }

        public async Task<string> UploadImageToGupshup(string partnerAppToken, string appId, IFormFile imageFile)
        {
            string appId1 = "your-app-id";
            string partnerAppToken1 = "your-partner-app-token";
            string endpoint = $"https://partner.gupshup.io/partner/app/{appId}/upload/media";
            string filePath = "path/to/your/image.jpg"; 

            try
            {
             
                var formDataRequest = new MultipartFormDataRequest
                {
                    FileStream = File.OpenRead(filePath),
                    FileName = Path.GetFileName(filePath),
                    FormFields = new Dictionary<string, string> { { "file_type", "image" } }};

              
                var oauths = new Dictionary<string, string>
                {
                    { "Authorization", partnerAppToken }
                };

                string response = APICallingHelper.BindMainAPIRequestModel<string, MultipartFormDataRequest>(
                    EndPoint: endpoint,
                    MethodType: "post",
                    RequestBody: formDataRequest,
                    OAUths: oauths,
                    APIType: "GupshupMediaUpload",
                    QuerryStringParameters: null,
                    AddLogs: true,
                    IsExternalAPI: true,
                    BodyType: "multipart"
                );

                var handleIdStart = response.IndexOf("\"message\":\"") + 10;
                var handleIdEnd = response.IndexOf("\"", handleIdStart);
                var handleId = response.Substring(handleIdStart, handleIdEnd - handleIdStart);

                Console.WriteLine($"Handle ID: {handleId}");

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<string> SendWhatsAppMessage(IFormFile File)
        {
            if (File == null || File.Length == 0)
            {
                return "Please upload a valid CSV file.";
            }
            var results = new List<string>();;

            try
            {
                var result = _campaignRepository.ActiveCampaign(true).Result;
                using (var stream = File.OpenReadStream())
                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<WhatsAppRecipient>();

                    foreach (var record in records)
                    {
                        if (!string.IsNullOrWhiteSpace(record.phone.ToString()) && !string.IsNullOrWhiteSpace(record.Name.ToString()) && !string.IsNullOrWhiteSpace(record.Description.ToString()))
                        {
                            var templateId = _configuration["GupshupConfiguration:templateId"];
                            var source = _configuration["GupshupConfiguration:source"];
                            var appName = _configuration["GupshupConfiguration:appName"];
                            var apiKey = _configuration["GupshupConfiguration:apiKey"];
                            var SendSMSUrl = _configuration["GupshupConfiguration:SendSMSUrl"];
                            var destination = record.phone.ToString();

                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Add("apikey", apiKey);
                            var formData = new NameValueCollection
                            {
                            { "channel", "whatsapp" },
                            { "source", source },
                            { "destination", destination },
                            { "src.name", appName },
                            { "template", "{\"id\":\"" + templateId + "\",\"params\":[\"" + record.Name + "\", \"" + result.Desciption + "\"]}" },
                            { "message", "{\"image\":{\"link\":\"" + result.ImagePath + "\"},\"type\":\"image\"}" }
                            };

                            var content = new FormUrlEncodedContent(ToDictionary(formData));
                            var response = await client.PostAsync(SendSMSUrl, content);

                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                
                            }
                            else
                            {
                                return $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                            }
                        }
                       
                    }
                }

                return "Message sent successfully!";
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

        private static Dictionary<string, string> ToDictionary(NameValueCollection collection)
        {
            var dict = new Dictionary<string, string>();
            foreach (string key in collection.AllKeys)
            {
                dict.Add(key, collection[key]);
            }
            return dict;
        }

        public  async Task<List<Dictionary<string, string>>> ReadCsvAsync(IFormFile file)
        {
            var result = new List<Dictionary<string, string>>();

            using (var stream = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            {
                string? headerLine = await stream.ReadLineAsync();
                if (headerLine == null)
                    return result;

                var headers = headerLine.Split(',');
                    
                while (!stream.EndOfStream)
                {
                    var line = await stream.ReadLineAsync();
                    if (line == null) continue;

                    var values = line.Split(',');
                    var rowDict = new Dictionary<string, string>();

                    for (int i = 0; i < headers.Length; i++)
                    {
                        string key = headers[i].Trim();
                        string value = i < values.Length ? values[i].Trim() : "";
                        rowDict[key] = value;
                    }

                    result.Add(rowDict);
                }
            }

            return result;
        }


    }
}
