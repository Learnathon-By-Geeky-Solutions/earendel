using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

    public static class GetJobRequiredSkillEndpoint
    {
        internal static RouteHandlerBuilder MapGetJobRequiredSkillEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    var response = await mediator.Send(new GetJobRequiredSkillRequest(id));
                    return Results.Ok(response);
                })
                .WithName(nameof(GetJobRequiredSkillEndpoint))
                .WithSummary("Gets a Job Required Skill by id")
                .WithDescription("Retrieves a Job Required Skill association by its identifier")
                .Produces<JobRequiredSkillResponse>()
                .RequirePermission("Permissions.JobRequiredSkills.View")
                .MapToApiVersion(1);
        }
    }
}
