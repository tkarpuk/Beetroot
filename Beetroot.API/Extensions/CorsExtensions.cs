using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beetroot.API.Extensions
{
    public static class CorsExtensions
    {
        public static void AddCorsServiceExt(this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy("AllowOrigin", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyOrigin();
                    policy.AllowAnyMethod();
                })
            );
        }

        public static void UseCorsMiddlewareExt(this IApplicationBuilder app)
        {
            app.UseCors("AllowOrigin");
        }
    }
}
