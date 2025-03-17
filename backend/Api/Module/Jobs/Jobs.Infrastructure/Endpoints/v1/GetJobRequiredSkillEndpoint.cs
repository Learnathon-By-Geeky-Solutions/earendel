using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1
{
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
                .RequirePermission("Permissions.JobRequiredSkill.View")
                .MapToApiVersion(1);
        }
    }
}
