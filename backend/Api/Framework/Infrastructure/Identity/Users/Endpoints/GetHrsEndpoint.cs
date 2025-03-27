using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;
[ExcludeFromCodeCoverage]

public static class GetHrEndpoint
{
    internal static RouteHandlerBuilder MapGetHrsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/hr", (IUserService service, string? search, string? sortBy, int pageNumber, int pageSize, string? sortDirection) =>
        {
            return service.GetHrsAsync(search, sortBy, sortDirection, pageNumber, pageSize, CancellationToken.None);
        })
        .WithName(nameof(GetHrEndpoint))
        .WithSummary("Get all hr with details")
        // .RequirePermission("Permissions.Users.View")
        .WithDescription("Get all hr with details.");
    }
}
