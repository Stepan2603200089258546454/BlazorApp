using DataContext.Models.Cloud;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.EntityConfiguration.Cloud
{
    internal class PersonalCloudConfiguration : IEntityTypeConfiguration<PersonalCloud>
    {
        public void Configure(EntityTypeBuilder<PersonalCloud> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany(x => x.PersonalClouds).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.Items).WithOne(x => x.PersonalCloud).HasForeignKey(x => x.PersonalCloudId);
        }
    }
}
