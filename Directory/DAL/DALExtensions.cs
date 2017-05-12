using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public static class DALExtensions
    {
        internal static void AddConfiguration<T>(this ModelBuilder builder, IModelConfig<T> config)
            where T : class, IEntity =>
            config.Configure(builder.Entity<T>());
    }
}
