using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

    public static class GetJobRequiredSubskillEndpoint
    {
        internal static RouteHandlerBuilder MapGetJobRequiredSubskillEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    var response = await mediator.Send(new GetJobRequiredSubskillRequest(id));
                    return Results.Ok(response);
                })
                .WithName(nameof(GetJobRequiredSubskillEndpoint))
                .WithSummary("Gets a Job Required Subskill by id")
                .WithDescription("Retrieves a Job Required Subskill association by its identifier")
                .Produces<JobRequiredSubskillResponse>()
                .RequirePermission("Permissions.JobRequiredSubskills.View") 
                .MapToApiVersion(1);
        }
    }
}
