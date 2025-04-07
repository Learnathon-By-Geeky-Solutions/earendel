using TalentMesh.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Module.Evaluator.Application.Interviewer.Delete.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]
    public static class DeleteInterviewerAvailabilityEndpoint
    {
        internal static RouteHandlerBuilder MapInterviewerAvailabilityDeleteEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    await mediator.Send(new DeleteInterviewerAvailabilityCommand(id));
                    return Results.NoContent();
                })
                .WithName(nameof(DeleteInterviewerAvailabilityEndpoint))
                .WithSummary("Deletes Interviewer Availability by id")
                .WithDescription("Deletes Interviewer Availability by id")
                .Produces(StatusCodes.Status204NoContent)
                // .RequirePermission("Permissions.Interviewer.Delete")
                .MapToApiVersion(1);
        }
    }
}
