using Gupshupcampainmanager.Models;
using Gupshupcampainmanager.Repository.Interface;

namespace Gupshupcampainmanager.Service
{
    public class AuthService: IAuthService
    {
        public async Task<LoginResponse> Login(LoginModel model)
        { 
            // model.request
            if(model.UserName =="Admin" && model.Password == "Admin@123")
            {
                return new LoginResponse { 
                        
                    IsSuccess = true,
                    UserName= model.UserName,
                    Role ="Admin",
                };

            }
            return new LoginResponse { 
                
                IsSuccess=false,
                Message = "Invalid Credentials",
            };
        
        }
    }
}
