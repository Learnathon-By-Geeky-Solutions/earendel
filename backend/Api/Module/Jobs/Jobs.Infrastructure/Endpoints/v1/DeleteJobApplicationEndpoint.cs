using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobApplication.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class DeleteJobApplicationEndpoint
{
    internal static RouteHandlerBuilder MapJobApplicationDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new DeleteJobApplicationCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(DeleteJobApplicationEndpoint))
            .WithSummary("Deletes a Job Application by id")
            .WithDescription("Deletes a Job Application by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.JobApplication.Delete")
            .MapToApiVersion(1);
    }
}