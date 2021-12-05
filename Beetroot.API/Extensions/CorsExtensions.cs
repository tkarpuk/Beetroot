using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Beetroot.API.Extensions
{
    public static class CorsExtensions
    {
        const string PolicyTitle = "AllowOrigin";

        public static void AddCorsServiceExt(this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy(PolicyTitle, policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyOrigin();
                    policy.AllowAnyMethod();
                })
            );
        }

        public static void UseCorsMiddlewareExt(this IApplicationBuilder app)
        {
            app.UseCors(PolicyTitle);
        }
    }
}
