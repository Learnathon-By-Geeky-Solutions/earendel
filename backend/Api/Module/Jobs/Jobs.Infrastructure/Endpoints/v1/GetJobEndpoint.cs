using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.Jobs.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1;
public static class GetJobEndpoint
{
    internal static RouteHandlerBuilder MapGetJobEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetJobRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetJobEndpoint))
            .WithSummary("gets product by id")
            .WithDescription("gets prodct by id")
            .Produces<JobResponse>()
            .RequirePermission("Permissions.Products.View")
            .MapToApiVersion(1);
    }
}
