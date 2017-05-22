using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using Common;
using Auth;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Backend.Modules
{
    public class AuthModule : IModule
    {
        private IConfigurationService _configService;
        private Encoding _encoding;

        public AuthModule(IConfigurationService configService, Encoding encoding)
        {
            _configService = configService;
            _encoding = encoding;
        }

        public void Register(IServiceCollection services)
        {
            services.AddSingleton<HashAlgorithm>(SHA256.Create());
            AuthConfig config = _configService.GetConfig<AuthConfig>("Auth");
            var key = new SymmetricSecurityKey(_encoding.GetBytes(config.Key));
            config.Credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            services.AddSingleton<AuthConfig>(config);

            services.AddSingleton<IAuthRule, EmailRule>();
            services.AddSingleton<IAuthRule, PasswordRule>();
            services.AddScoped<IAuthRule, EmailConflictRule>();

            services.AddScoped<IGrantProvider, PasswordGrantProvider>();
            services.AddSingleton<IGrantProvider, RefreshTokenGrantProvider>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
