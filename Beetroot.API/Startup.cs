using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Beetroot.DAL;
using Microsoft.Extensions.Configuration;
using Beetroot.BLL.Interfaces;
using Beetroot.BLL.Services;
using Beetroot.API.Services;

namespace Beetroot.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddTransient<IMessageService, MessageService>();
            services.AddDbContextExt(_configuration.GetConnectionString("DefaultConnection"));
            services.AddHostedService<UdpHostedService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
