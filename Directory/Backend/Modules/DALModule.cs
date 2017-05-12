using Microsoft.Extensions.DependencyInjection;
using DAL;
using Microsoft.EntityFrameworkCore;
using Common;

namespace Backend.Modules
{
    public class DALModule : IModule
    {
        private IConfigurationService _configService;

        public DALModule(IConfigurationService configService)
        {
            _configService = configService;
        }

        public void Register(IServiceCollection services)
        {
            var config = _configService.GetConfig<DALConfig>("DAL");

            services.AddScoped<IDataService, DataService>();
            services.AddDbContext<DataService>(options => options.UseSqlServer(
                config.ConnectionString, b => b.MigrationsAssembly("Backend")));
        }

        private class DALConfig
        {
            public string ConnectionString { get; private set; }
        }
    }
}
