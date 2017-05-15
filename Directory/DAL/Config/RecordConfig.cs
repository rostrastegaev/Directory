using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    internal class RecordConfig : IModelConfig<Record>
    {
        public void Configure(EntityTypeBuilder<Record> builder)
        {
            builder.ToTable("tbRecord");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.FirstName).HasColumnName("fldFirstName");
            builder.Property(r => r.LastName).HasColumnName("fldLastName");
            builder.Property(r => r.Surname).HasColumnName("fldSurname");
            builder.Property(r => r.DateOfBirth).HasColumnName("fldDateOfBirth");
            builder.Property(r => r.Info).HasColumnName("fldInfo");
            builder.Property(r => r.Phone).HasColumnName("fldPhone");
            builder.HasOne(r => r.User)
                .WithMany(u => u.Records)
                .HasForeignKey(r => r.UserId)
                .HasPrincipalKey(u => u.Id)
                .IsRequired();
        }
    }
}
