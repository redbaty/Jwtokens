using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jwtokens
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtokens(this IServiceCollection services,
            Func<JwtokensOptions, JwtokensOptions> optionsFunc = null)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var appSettingsSection = configuration.GetSection("JwtokenSettings");
            var options = new JwtokensOptions();

            services.Configure<JwtokensSettings>(appSettingsSection);
            services.AddTransient<JwtService>();
            services.AddSingleton((optionsFunc ?? (jwtokensOptions => jwtokensOptions)).Invoke(options));
            return services;
        }

        public static IServiceCollection AddJwtokensAuthentication(this IServiceCollection services)
        {
            var jwtService = services.BuildServiceProvider().GetService<JwtService>();
            jwtService.AddAuthentication(services);
            return services;
        }
    }
}