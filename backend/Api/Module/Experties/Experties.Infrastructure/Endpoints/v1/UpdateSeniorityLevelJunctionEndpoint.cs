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
            .MapPut("/{id:guid}", async (Guid id, UpdateSeniorityLevelJunctionCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateSeniorityLevelJunctionEndpoint))
            .WithSummary("update a seniority level junction")
            .WithDescription("update a seniority level junction")
            .Produces<UpdateSeniorityLevelJunctionResponse>()
            // .RequirePermission("Permissions.SeniorityLevelJunctions.Update")
            .MapToApiVersion(1);
    }
}