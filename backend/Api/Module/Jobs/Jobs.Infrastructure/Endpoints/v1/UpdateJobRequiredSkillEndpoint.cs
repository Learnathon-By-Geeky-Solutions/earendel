using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1
{
    public static class UpdateJobRequiredSkillEndpoint
    {
        internal static RouteHandlerBuilder MapJobRequiredSkillUpdateEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPut("/{id:guid}", async (Guid id, UpdateJobRequiredSkillCommand request, ISender mediator) =>
                {
                    if (id != request.Id)
                        return Results.BadRequest();

                    var response = await mediator.Send(request);
                    return Results.Ok(response);
                })
                .WithName(nameof(UpdateJobRequiredSkillEndpoint))
                .WithSummary("Updates a Job Required Skill")
                .WithDescription("Updates a Job Required Skill association by its identifier")
                .Produces<UpdateJobRequiredSkillResponse>()
                .RequirePermission("Permissions.JobRequiredSkill.Update")
                .MapToApiVersion(1);
        }
    }
}
