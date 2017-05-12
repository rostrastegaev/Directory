using Microsoft.Extensions.DependencyInjection;
using BL;
using DAL;

namespace Backend.Modules
{
    public class BLModule : IModule
    {
        public void Register(IServiceCollection services)
        {
            services.AddSingleton<IFetchProvider<Record>>(new RecordFetchProvider());
            services.AddSingleton<IRecordOperations, RecordOperations>();
        }
    }
}
