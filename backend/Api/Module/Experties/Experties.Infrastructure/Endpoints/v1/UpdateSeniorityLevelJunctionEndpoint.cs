using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;

[ExcludeFromCodeCoverage]
public static class UpdateSeniorityLevelJunctionEndpoint
{
    internal static RouteHandlerBuilder MapSeniorityLevelJunctionUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/", async (UpdateSeniorityLevelJunctionCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateSeniorityLevelJunctionEndpoint))
            .WithSummary("Update seniority level junctions for a skill")
            .WithDescription("Updates the seniority level junctions for a given skill")
            .Produces<UpdateSeniorityLevelJunctionResponse>()
            .RequirePermission("Permissions.SeniorityLevelJunctions.Update")    // ← added
            .MapToApiVersion(1);
    }
}
