using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

    public static class DeleteJobRequiredSubskillEndpoint
    {
        internal static RouteHandlerBuilder MapJobRequiredSubskillDeleteEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    await mediator.Send(new DeleteJobRequiredSubskillCommand(id));
                    return Results.NoContent();
                })
                .WithName(nameof(DeleteJobRequiredSubskillEndpoint))
                .WithSummary("Deletes a Job Required Subskill by id")
                .WithDescription("Deletes a Job Required Subskill by id")
                .Produces(StatusCodes.Status204NoContent)
                .RequirePermission("Permissions.JobRequiredSubskills.Delete")
                .MapToApiVersion(1);
        }
    }
}
