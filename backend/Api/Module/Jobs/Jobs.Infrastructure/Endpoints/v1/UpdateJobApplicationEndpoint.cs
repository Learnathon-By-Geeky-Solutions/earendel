using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Job.Application.JobApplication.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class UpdateJobApplicationEndpoint
{
    internal static RouteHandlerBuilder MapJobApplicationUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateJobApplicationCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest();

                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateJobApplicationEndpoint))
            .WithSummary("Updates a job application")
            .WithDescription("Updates a job application by its identifier")
            .Produces<UpdateJobApplicationResponse>()
            .RequirePermission("Permissions.JobApplications.Update")
            .MapToApiVersion(1);
    }
}
