using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
public static class CreateSeniorityLevelJunctionEndpoint
{
    internal static RouteHandlerBuilder MapSeniorityLevelJunctionCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateSeniorityLevelJunctionCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateSeniorityLevelJunctionEndpoint))
            .WithSummary("creates a seniority level junction")
            .WithDescription("creates a seniority level junction")
            .Produces<CreateSeniorityLevelJunctionResponse>()
            // .RequirePermission("Permissions.SeniorityLevelJunctions.Create")
            .MapToApiVersion(1);
    }
}