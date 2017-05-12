using Microsoft.Extensions.DependencyInjection;

namespace Backend
{
    public interface IModule
    {
        void Register(IServiceCollection services);
    }
}
