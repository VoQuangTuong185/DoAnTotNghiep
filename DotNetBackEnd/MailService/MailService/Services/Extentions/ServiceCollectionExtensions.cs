using MailService.Services.Business;
using MailService.Services.Contracts;

namespace MailService.Services.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAPIServices(this IServiceCollection services)
        {
            services.AddTransient<IMailService, MailServices>();
            services.AddTransient<IMailContent, MailContent>();
        }
    }
}
