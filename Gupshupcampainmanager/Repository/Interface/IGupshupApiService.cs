namespace Gupshupcampainmanager.Repository.Interface
{
    public interface IGupshupApiService
    {
        Task<string> UploadImageToGupshup(string partnerAppToken, string appId, IFormFile imageFile);
        Task<string> SendWhatsAppMessage(string apiKey, string source, string destination, string appName, string templateId, string imageHandleId, string offerText, string SendSMSUrl);
        Task<List<Dictionary<string, string>>> ReadCsvAsync(IFormFile file);

    }
}
