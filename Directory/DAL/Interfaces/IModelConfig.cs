using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    internal interface IModelConfig<T> where T : class, IEntity
    {
        void Configure(EntityTypeBuilder<T> builder);
    }
}
