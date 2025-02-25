using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Skills.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
public static class DeleteSkillEndpoint
{
    internal static RouteHandlerBuilder MapSkillDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteSkillCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteSkillEndpoint))
            .WithSummary("deletes skill by id")
            .WithDescription("deletes skill by id")
            .Produces(StatusCodes.Status204NoContent)
            // .RequirePermission("Permissions.Products.Delete")
            .MapToApiVersion(1);
    }
}
