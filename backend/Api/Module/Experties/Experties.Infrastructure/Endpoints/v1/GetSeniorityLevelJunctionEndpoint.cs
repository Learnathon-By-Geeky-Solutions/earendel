using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
public static class GetSeniorityLevelJunctionEndpoint
{
    internal static RouteHandlerBuilder MapGetSeniorityLevelJunctionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetSeniorityLevelJunctionRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetSeniorityLevelJunctionEndpoint))
            .WithSummary("gets seniority level junction by id")
            .WithDescription("gets seniority level junction by id")
            .Produces<SeniorityLevelJunctionResponse>()
            // .RequirePermission("Permissions.Jobs.View")
            .MapToApiVersion(1);
    }
}