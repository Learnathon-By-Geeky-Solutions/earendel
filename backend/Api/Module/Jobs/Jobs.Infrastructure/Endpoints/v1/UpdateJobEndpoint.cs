using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.Jobs.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1;
public static class UpdateJobEndpoint
{
    internal static RouteHandlerBuilder MapJobUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateJobCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateJobEndpoint))
            .WithSummary("update a product")
            .WithDescription("update a product")
            .Produces<UpdateJobResponse>()
            .RequirePermission("Permissions.Job.Update")
            .MapToApiVersion(1);
    }
}
