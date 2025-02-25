using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.Jobs.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1;
public static class CreateJobEndpoint
{
    internal static RouteHandlerBuilder MapJobCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateJobCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateJobEndpoint))
            .WithSummary("creates a product")
            .WithDescription("creates a product")
            .Produces<CreateJobResponse>()
            .RequirePermission("Permissions.Jobs.Create")
            .MapToApiVersion(1);
    }
}
