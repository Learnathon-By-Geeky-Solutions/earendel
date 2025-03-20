using Serilog;
using Serilog.Core;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Logging.Serilog;

[ExcludeFromCodeCoverage]

public static class StaticLogger
{
    public static void EnsureInitialized()
    {
        if (Log.Logger is not Logger)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.OpenTelemetry()
                .CreateLogger();
        }
    }
}
