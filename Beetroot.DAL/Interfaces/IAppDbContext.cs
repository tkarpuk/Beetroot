using Beetroot.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Beetroot.DAL.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Address> Addresses { get; set; }
        DbSet<Message> Messages { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
