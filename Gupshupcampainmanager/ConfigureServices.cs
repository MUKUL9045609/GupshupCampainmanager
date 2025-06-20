using Microsoft.AspNetCore.Mvc;

namespace Gupshupcampainmanager
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebUIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddAuthorization();
            
            //services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true);

            return services;
        }
    }
}
