using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class DeleteSeniorityLevelJunctionEndpoint
{
    internal static RouteHandlerBuilder MapSeniorityLevelJunctionDeletionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new DeleteSeniorityLevelJunctionCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(DeleteSeniorityLevelJunctionEndpoint))
            .WithSummary("deletes a seniority level junction")
            .WithDescription("deletes a seniority level junction")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.SeniorityLevelJunctions.Delete")    // ← added
            .MapToApiVersion(1);
    }
}