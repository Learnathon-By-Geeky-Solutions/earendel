using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

    public static class CreateJobRequiredSubskillEndpoint
    {
        internal static RouteHandlerBuilder MapJobRequiredSubskillCreationEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPost("/", async (CreateJobRequiredSubskillCommand request, ISender mediator) =>
                {
                    var response = await mediator.Send(request);
                    return Results.Ok(response);
                })
                .WithName(nameof(CreateJobRequiredSubskillEndpoint))
                .WithSummary("Creates a Job Required Subskill")
                .WithDescription("Creates a Job Required Subskill association for a job")
                .Produces<CreateJobRequiredSubskillResponse>()
                .RequirePermission("Permissions.Jobs.Create")
                .MapToApiVersion(1);
        }
    }
}
