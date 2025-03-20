using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

    public static class DeleteJobRequiredSkillEndpoint
    {
        internal static RouteHandlerBuilder MapJobRequiredSkillDeleteEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    await mediator.Send(new DeleteJobRequiredSkillCommand(id));
                    return Results.NoContent();
                })
                .WithName(nameof(DeleteJobRequiredSkillEndpoint))
                .WithSummary("Deletes a Job Required Skill by id")
                .WithDescription("Deletes a Job Required Skill by id")
                .Produces(StatusCodes.Status204NoContent)
                .RequirePermission("Permissions.JobRequiredSkill.Delete")
                .MapToApiVersion(1);
        }
    }
}
