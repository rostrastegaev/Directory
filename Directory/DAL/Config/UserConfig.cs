using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    internal class UserConfig : IModelConfig<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("tbUser");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).HasColumnName("fldEmail");
            builder.Property(u => u.Password).HasColumnName("fldPassword");
        }
    }
}
