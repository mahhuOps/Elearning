using EmailService.Interfaces;
using EmailService.Implements;
using Microsoft.Extensions.DependencyInjection;

namespace EmailService
{
    public static class StartupExtension
    {
        public static void UseEmailService(this IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailService.Implements.EmailService>();
        }
    }
}
