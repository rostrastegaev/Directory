using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Backend.Modules;
using Auth;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.AspNetCore.Http;

namespace Backend
{
    public class Startup
    {
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddAuthorization();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var commonModule = new CommonModule(Configuration, _env);
            commonModule.Register(services);
            new DALModule(commonModule.ConfigurationService).Register(services);
            new AuthModule(commonModule.ConfigurationService, commonModule.DefaultEncoding).Register(services);
            new BLModule().Register(services);
        }

        public void Configure(IApplicationBuilder app,
            ILoggerFactory logFactory,
            AuthConfig authConfig,
            Encoding encoding)
        {
            logFactory.AddFile(Configuration.GetSection("Logging"));

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = authConfig.Issuer,
                    ValidAudience = authConfig.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(encoding.GetBytes(authConfig.Key)),
                    ClockSkew = TimeSpan.Zero
                }
            });
            
            app.UseMvc();
        }
    }
}
