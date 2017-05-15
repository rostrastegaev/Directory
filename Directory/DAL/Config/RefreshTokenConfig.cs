using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class RefreshTokenConfig : IModelConfig<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("tbRefreshToken");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.UserId).HasColumnName("fldUserId");
            builder.Property(t => t.Token).HasColumnName("fldToken");
            builder.HasOne(t => t.User)
                .WithOne(u => u.Token)
                .HasForeignKey<RefreshToken>(t => t.UserId)
                .HasPrincipalKey<User>(u => u.Id)
                .IsRequired();
        }
    }
}
