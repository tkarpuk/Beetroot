using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;

namespace Beetroot.API.Extensions
{
    public static class SwaggerExtensions
    {
        const string AppTitle = "Beetroot Test Task API";
        public static void AddSwaggerServiceExt(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = AppTitle, Version = "v1" });
            });
        }

        public static void UseSwaggerMiddlewareExt(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", AppTitle);
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
