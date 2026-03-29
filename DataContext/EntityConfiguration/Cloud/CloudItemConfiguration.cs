using DataContext.Models.Cloud;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.EntityConfiguration.Cloud
{
    internal class CloudItemConfiguration : IEntityTypeConfiguration<CloudItem>
    {
        public void Configure(EntityTypeBuilder<CloudItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.PersonalCloud).WithMany(x => x.Items).HasForeignKey(x => x.PersonalCloudId);
            builder.HasOne(x => x.FileData).WithOne(x => x.CloudItem).HasForeignKey<CloudItem>(x => x.FileDataId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Children).WithOne(c => c.Parrent).HasForeignKey(c => c.ParrentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
