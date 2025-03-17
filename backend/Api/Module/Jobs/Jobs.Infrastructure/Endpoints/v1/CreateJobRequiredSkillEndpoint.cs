using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1
{
    public static class CreateJobRequiredSkillEndpoint
    {
        internal static RouteHandlerBuilder MapJobRequiredSkillCreationEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPost("/", async (CreateJobRequiredSkillCommand request, ISender mediator) =>
                {
                    var response = await mediator.Send(request);
                    return Results.Ok(response);
                })
                .WithName(nameof(CreateJobRequiredSkillEndpoint))
                .WithSummary("Creates a Job Required Skill")
                .WithDescription("Creates a Job Required Skill association for a job")
                .Produces<CreateJobRequiredSkillResponse>()
                .RequirePermission("Permissions.JobRequiredSkill.Create")
                .MapToApiVersion(1);
        }
    }
}
