using WebAppAPI.Services.Business;
using WebAppAPI.Services.Contracts;

namespace RBVH.HRL.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAPIServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminService, AdminService>();           
        }
    }
}
