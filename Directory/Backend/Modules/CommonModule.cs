using Microsoft.Extensions.DependencyInjection;
using Common;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Backend.Modules
{
    public class CommonModule : IModule
    {
        private IConfiguration _config;

        public IConfigurationService ConfigurationService { get; }
        public Encoding DefaultEncoding { get; }

        public CommonModule(IConfiguration config)
        {
            _config = config;
            ConfigurationService = new ConfigurationService(_config);
            DefaultEncoding = Encoding.Unicode;
        }

        public void Register(IServiceCollection services)
        {
            services.AddSingleton<Encoding>(DefaultEncoding);
            services.AddSingleton<IConfigurationService>(ConfigurationService);
        }
    }
}
