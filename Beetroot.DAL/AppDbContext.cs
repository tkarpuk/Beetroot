using Beetroot.DAL.Configurations;
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
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
