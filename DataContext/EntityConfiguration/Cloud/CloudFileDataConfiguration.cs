using DataContext.Models.Cloud;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.EntityConfiguration.Cloud
{
    internal class CloudFileDataConfiguration : IEntityTypeConfiguration<CloudFileData>
    {
        public void Configure(EntityTypeBuilder<CloudFileData> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.CloudItem).WithOne(x => x.FileData).HasForeignKey<CloudItem>(x => x.FileDataId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
