using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobApplication.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class GetJobApplicationEndpoint
{
    internal static RouteHandlerBuilder MapGetJobApplicationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetJobApplicationRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetJobApplicationEndpoint))
            .WithSummary("Gets a job application by id")
            .WithDescription("Gets a job application by id")
            .Produces<JobApplicationResponse>()
            .RequirePermission("Permissions.JobApplications.View")
            .MapToApiVersion(1);
    }
}
