namespace Gupshupcampainmanager.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }

        public int ExpiryInsecond { get; set; }
        public string Message { get; set; }
    }


}
