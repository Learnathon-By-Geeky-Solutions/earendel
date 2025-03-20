using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.SignalR
{
    [ExcludeFromCodeCoverage]

    public static class SignalRExtensions
    {
        // Register SignalR services
        public static IServiceCollection ConfigureSignalR(this IServiceCollection services)
        {
            services.AddSignalR();
            return services;
        }

        // Configure SignalR hubs in the application pipeline
        public static IApplicationBuilder UseSignalRHubs(this IApplicationBuilder app)
        {
            app.UseRouting(); // Ensure routing is enabled

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notifications");
            });

            return app;
        }
    }
}
