using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Beetroot.DAL;
using Microsoft.Extensions.Configuration;
using Beetroot.BLL.Interfaces;
using Beetroot.BLL.Services;
using Beetroot.API.Services;
using Beetroot.API.Extensions;
using Microsoft.Extensions.Logging;

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

            services.AddSwaggerServiceExt();
            services.AddCorsServiceExt();

            services.AddTransient<IMessageService, MessageService>();
            services.AddDbContextExt(_configuration.GetConnectionString("DefaultConnection"));
            services.AddHostedService<UdpHostedService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandlerExt(logger);
            app.UseCorsMiddlewareExt();
            app.UseRouting();
            app.UseSwaggerMiddlewareExt();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
