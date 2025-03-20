using TalentMesh.Framework.Infrastructure;
using TalentMesh.Framework.Infrastructure.Logging.Serilog;
using TalentMesh.WebApi.Host;
using Serilog;
using System.Diagnostics.CodeAnalysis;

StaticLogger.EnsureInitialized();
Log.Information("Server booting up...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureTMFramework();
    builder.RegisterModules();

    var app = builder.Build();

    app.UseFshFramework();
    app.UseModules();
    await app.RunAsync();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception occurred"); // Proper exception logging
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server shutting down...");
    await Log.CloseAndFlushAsync();
}