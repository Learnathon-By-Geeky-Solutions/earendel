using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.SecurityHeaders
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        internal static IServiceCollection ConfigureSecurityHeaders(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SecurityHeaderOptions>(config.GetSection(nameof(SecurityHeaderOptions)));
            return services;
        }

        internal static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<SecurityHeaderOptions>>().Value;

            if (options.Enable)
            {
                app.Use(async (context, next) =>
                {
                    if (!context.Response.HasStarted)
                    {
                        var headers = context.Response.Headers;

                        // Define header mappings as tuples: (header key, header value)
                        var headerMappings = new List<(string Key, string? Value)>
                        {
                            ("X-Frame-Options", options.Headers.XFrameOptions),
                            ("X-Content-Type-Options", options.Headers.XContentTypeOptions),
                            ("Referer", options.Headers.ReferrerPolicy),
                            ("Permissions-Policy", options.Headers.PermissionsPolicy),
                            ("X-XSS-Protection", options.Headers.XXSSProtection),
                            ("Content-Security-Policy", options.Headers.ContentSecurityPolicy),
                            ("Strict-Transport-Security", options.Headers.StrictTransportSecurity)
                        };

                        foreach (var (key, value) in headerMappings)
                        {
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                headers[key] = value;
                            }
                        }
                    }

                    await next.Invoke();
                });
            }

            return app;
        }
    }
}
