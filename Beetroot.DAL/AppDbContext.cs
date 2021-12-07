using Beetroot.DAL.Entities;
using Beetroot.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Beetroot.DAL
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Message> Messages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasKey(a => a.Id);
            modelBuilder.Entity<Address>().HasMany(a => a.Messages).WithOne(m => m.IpAddress);


            modelBuilder.Entity<Message>().HasKey(m => m.Id);
            modelBuilder.Entity<Message>().Property(m => m.TextMessage).HasMaxLength(200);
            modelBuilder.Entity<Message>().HasIndex(m => m.DateMessage);
            modelBuilder.Entity<Message>().HasOne(m => m.IpAddress).WithMany(a => a.Messages)
                        .HasForeignKey(m => m.AddressId)
                        .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
