using Gupshupcampainmanager.Models;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using Gupshupcampainmanager.Helpers;
using Gupshupcampainmanager.Repository.Interface;

namespace Gupshupcampainmanager.Service
{
    public class GupshupApiService : IGupshupApiService
    {
        private static readonly HttpClient client = new HttpClient();

        private IConfiguration _configuration { get; }

        public GupshupApiService(IConfiguration configuration)
        {
            _configuration = configuration;
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

        public async Task<string> SendWhatsAppMessage(string apiKey, string source, string destination, string appName, string templateId, string imageHandleId, string offerText, string SendSMSUrl)
        {
            try
            {

                templateId = _configuration["GupshupConfiguration:templateId"];
                source = _configuration["GupshupConfiguration:source"];
                appName = _configuration["GupshupConfiguration:appName"];
                apiKey = _configuration["GupshupConfiguration:apiKey"];
                SendSMSUrl = _configuration["GupshupConfiguration:SendSMSUrl"];
                imageHandleId = offerText;
                destination = "919552065205";

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("apikey", apiKey);
                var formData = new NameValueCollection
                {
                { "channel", "whatsapp" },
                { "source", source },
                { "destination", destination },
                { "src.name", appName },
                { "template", "{\"id\":\"" + templateId + "\",\"params\":[\"" + imageHandleId + "\", \"" + offerText + "\"]}" },
                { "message", "{\"image\":{\"link\":\"https://hrms.fortune4.org/images/ohrm_branding.png\"},\"type\":\"image\"}" }
                };

                var content = new FormUrlEncodedContent(ToDictionary(formData));
                var response = await client.PostAsync(SendSMSUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    return $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                }
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

    }
}
