using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;


namespace TalentMesh.Framework.Infrastructure.Identity.Tokens.Endpoints;
internal static class Extensions
{
    [ExcludeFromCodeCoverage]
    public static IEndpointRouteBuilder MapTokenEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRefreshTokenEndpoint();
        app.MapTokenGenerationEndpoint();
        app.MapLogoutEndpoint();
        return app;
    }
    [ExcludeFromCodeCoverage]

    public static string GetIpAddress(this HttpContext context)
    {
        string ip = "N/A";
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var ipList))
        {
            ip = ipList.FirstOrDefault() ?? "N/A";
        }
        else if (context.Connection.RemoteIpAddress != null)
        {
            ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        return ip;

    }
}
