using Gupshupcampainmanager.Models;

namespace Gupshupcampainmanager.Repository.Interface
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginModel model);
    }
}
