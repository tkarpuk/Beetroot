using Beetroot.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beetroot.DAL.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasIndex(a => a.IpAddress).IsUnique();
            builder.HasMany(a => a.Messages).WithOne(m => m.IpAddress);
        }
    }
}
