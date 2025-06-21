namespace Gupshupcampainmanager.Repository.Interface
{
    public interface IGupshupApiService
    {
        Task<string> UploadImageToGupshup(string partnerAppToken, string appId, IFormFile imageFile);
        Task<string> SendWhatsAppMessage(IFormFile imageFile);
        Task<List<Dictionary<string, string>>> ReadCsvAsync(IFormFile file);


    }
}
