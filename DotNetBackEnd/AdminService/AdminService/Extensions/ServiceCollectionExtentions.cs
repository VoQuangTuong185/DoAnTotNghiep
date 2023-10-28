using WebAppAPI.Repositories;

namespace WebAppAPI.Extensions
{
    public static class ServiceCollectionExtentions
    {
        public static void AddAPIRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(APIRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
