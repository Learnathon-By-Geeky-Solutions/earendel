using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace TalentMesh.Framework.Infrastructure.Logging.Serilog;

public static class Extensions
{
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Host.UseSerilog((context, logger) =>
        {
            logger.WriteTo.OpenTelemetry(options =>
            {
                try
                {
                    options.Endpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];

                    // Get the headers and validate them
                    var headers = builder.Configuration["OTEL_EXPORTER_OTLP_HEADERS"]?.Split(',') ?? Array.Empty<string>();
                    foreach (var header in headers)
                    {
                        var headerParts = header.Split('=');

                        if (headerParts.Length == 2)
                        {
                            var (key, value) = (headerParts[0], headerParts[1]);
                            options.Headers.Add(key, value);
                        }
                        else
                        {
                            // Handle invalid header format here, logging or throwing a more specific exception
                            // For example, we can log the invalid header and continue processing other headers
                            Console.WriteLine($"Warning: Invalid header format '{header}'");
                        }
                    }

                    options.ResourceAttributes.Add("service.name", "apiservice");

                    // Handle the resource attribute and validate it
                    var otelResourceAttributes = builder.Configuration["OTEL_RESOURCE_ATTRIBUTES"]?.Split('=');
                    if (otelResourceAttributes?.Length == 2)
                    {
                        var (otelResourceAttribute, otelResourceAttributeValue) = (otelResourceAttributes[0], otelResourceAttributes[1]);
                        options.ResourceAttributes.Add(otelResourceAttribute, otelResourceAttributeValue);
                    }
                    else
                    {
                        // Handle invalid format for OTEL_RESOURCE_ATTRIBUTES gracefully
                        Console.WriteLine("Warning: Invalid OTEL_RESOURCE_ATTRIBUTES format.");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception if necessary, but avoid throwing a generic exception
                    Console.WriteLine($"Error during configuration setup: {ex.Message}");
                }

            });
            logger.ReadFrom.Configuration(context.Configuration);
            logger.Enrich.FromLogContext();
            logger.Enrich.WithCorrelationId();
            logger
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
                .MinimumLevel.Override("Finbuckle.MultiTenant", LogEventLevel.Warning)
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware"));
        });
        return builder;
    }
}
