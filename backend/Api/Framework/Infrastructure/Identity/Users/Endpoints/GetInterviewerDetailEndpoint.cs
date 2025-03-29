using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;
[ExcludeFromCodeCoverage]

public static class GetInterviewerDetailEndpoint
{
    internal static RouteHandlerBuilder MapGetInterviewerDetailEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/interviewer/{id:guid}", (string id, IUserService service) =>
        {
            return service.GetInterviewerDetailAsync(id, CancellationToken.None);
        })
        .WithName(nameof(GetInterviewerDetailEndpoint))
        .WithSummary("Get a particular interviewer with details")
        // .RequirePermission("Permissions.Users.View")
        .WithDescription("Get a particular interviewer with details.");
    }
}
