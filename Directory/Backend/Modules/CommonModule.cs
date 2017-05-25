using Microsoft.Extensions.DependencyInjection;
using Common;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace Backend.Modules
{
    public class CommonModule : IModule
    {
        private IConfiguration _config;
        private IHostingEnvironment _host;

        public IConfigurationService ConfigurationService { get; }
        public Encoding DefaultEncoding { get; }

        public CommonModule(IConfiguration config, IHostingEnvironment host)
        {
            _host = host;
            _config = config;
            ConfigurationService = new ConfigurationService(_config);
            DefaultEncoding = Encoding.Unicode;
        }

        public void Register(IServiceCollection services)
        {
            var viewService = new ViewService($"{_host.ContentRootPath}\\wwwroot\\app");
            viewService.Init();
            services.AddSingleton<IViewService>(viewService);
            services.AddSingleton<Encoding>(DefaultEncoding);
            services.AddSingleton<IConfigurationService>(ConfigurationService);
        }
    }
}
