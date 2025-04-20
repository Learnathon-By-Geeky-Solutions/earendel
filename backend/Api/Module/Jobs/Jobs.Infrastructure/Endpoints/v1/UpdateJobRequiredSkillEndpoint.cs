using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

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
                .RequirePermission("Permissions.Jobs.Update")
                .MapToApiVersion(1);
        }
    }
}
