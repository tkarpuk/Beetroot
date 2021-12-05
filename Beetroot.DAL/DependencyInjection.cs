using Beetroot.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Beetroot.DAL
{
    public static class DependencyInjection
    {
        public static void AddDbContextExt(this IServiceCollection services, string connectionString)
        {          
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Beetroot.API"))
                );

            services.AddScoped<IAppDbContext>(provider =>
                provider.GetService<AppDbContext>());
        }
    
    }
}
