using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.SubSkills.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class DeleteSubSkillEndpoint
{
    internal static RouteHandlerBuilder MapSubSkillDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteSubSkillCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteSubSkillEndpoint))
            .WithSummary("deletes subskill by id")
            .WithDescription("deletes subskill by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.SubSkills.Delete")    // ← added
            .MapToApiVersion(1);
    }
}
