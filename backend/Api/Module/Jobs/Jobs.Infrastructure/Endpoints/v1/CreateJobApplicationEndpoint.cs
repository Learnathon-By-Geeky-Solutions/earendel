using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobApplication.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1;
public static class CreateJobApplicationEndpoint
{
    internal static RouteHandlerBuilder MapJobApplicationCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateJobApplicationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateJobApplicationEndpoint))
            .WithSummary("Creates a Job Application")
            .WithDescription("Creates a Job Application")
            .Produces<CreateJobApplicationResponse>()
            .RequirePermission("Permissions.JobApplication.Create")
            .MapToApiVersion(1);
    }
}
