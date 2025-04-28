using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Seniorities.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class DeleteSeniorityEndpoint
{
    internal static RouteHandlerBuilder MapSeniorityDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteSeniorityCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteSeniorityEndpoint))
            .WithSummary("deletes seniority by id")
            .WithDescription("deletes seniority by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Seniorities.Delete")    // ← added
            .MapToApiVersion(1);
    }
}
