using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Rubrics.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class DeleteRubricEndpoint
{
    internal static RouteHandlerBuilder MapRubricDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteRubricCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteRubricEndpoint))
            .WithSummary("deletes Rubric by id")
            .WithDescription("deletes Rubric by id")
            .Produces(StatusCodes.Status204NoContent)
            // .RequirePermission("Permissions.Products.Delete")
            .MapToApiVersion(1);
    }
}
