using DataContext.Models.Cloud;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.EntityConfiguration.Cloud
{
    internal class GlobalCloudConfiguration : IEntityTypeConfiguration<GlobalCloud>
    {
        public void Configure(EntityTypeBuilder<GlobalCloud> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany(x => x.GlobalClouds).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.Items).WithOne(x => x.GlobalCloud).HasForeignKey(x => x.GlobalCloudId);
        }
    }
}
