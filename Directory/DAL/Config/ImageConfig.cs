using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    internal class ImageConfig : IModelConfig<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("tbImage");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.RecordId).HasColumnName("fldRecordId");
            builder.Property(i => i.File).HasColumnName("fldFile");
            builder.Property(i => i.ContentType).HasColumnName("fldContentType");
        }
    }
}
