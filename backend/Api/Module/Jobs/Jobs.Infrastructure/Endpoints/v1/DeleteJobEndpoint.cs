using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.Jobs.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class DeleteJobEndpoint
{
    internal static RouteHandlerBuilder MapJobDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteJobCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteJobEndpoint))
            .WithSummary("deletes job by id")
            .WithDescription("deletes job by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Jobs.Delete")
            .MapToApiVersion(1);
    }
}