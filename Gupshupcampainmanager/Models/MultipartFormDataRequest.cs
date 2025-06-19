namespace Gupshupcampainmanager.Models
{
    public class MultipartFormDataRequest
    {
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
        public Dictionary<string, string> FormFields { get; set; } = new Dictionary<string, string>();
    }
}
