using Beetroot.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beetroot.DAL.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.TextMessage).HasMaxLength(200);
            builder.HasIndex(m => m.DateMessage);
            builder.HasOne(m => m.IpAddress).WithMany(a => a.Messages)
                .HasForeignKey(m => m.AddressId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
