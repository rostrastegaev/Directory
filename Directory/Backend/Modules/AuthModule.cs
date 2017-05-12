using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using Common;
using Auth;

namespace Backend.Modules
{
    public class AuthModule : IModule
    {
        private IConfigurationService _configService;

        public AuthModule(IConfigurationService configService)
        {
            _configService = configService;
        }

        public void Register(IServiceCollection services)
        {
            services.AddSingleton<HashAlgorithm>(SHA256.Create());
            services.AddSingleton<AuthConfig>(_configService.GetConfig<AuthConfig>("Auth"));
            services.AddSingleton<IAuthRule, EmailRule>();
            services.AddSingleton<IAuthRule, PasswordRule>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
