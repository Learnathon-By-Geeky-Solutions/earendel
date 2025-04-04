using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;
[ExcludeFromCodeCoverage]

public static class GetInterviewerEndpoint
{
    internal static RouteHandlerBuilder MapGetInterviewersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/interviewer", (IUserService service, string? search, string? sortBy, int pageNumber, int pageSize, string? sortDirection) =>
        {
            return service.GetInterviewersAsync(search, sortBy, sortDirection, pageNumber, pageSize, CancellationToken.None);
        })
        .WithName(nameof(GetInterviewerEndpoint))
        .WithSummary("Get all interviewer with details")
        // .RequirePermission("Permissions.Users.View")
        .WithDescription("Get all interviewer with details.");
    }
}
