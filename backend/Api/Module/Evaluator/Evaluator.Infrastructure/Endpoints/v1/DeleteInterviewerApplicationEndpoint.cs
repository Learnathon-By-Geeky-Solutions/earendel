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
    public static class DeleteInterviewerApplicationEndpoint
    {
        internal static RouteHandlerBuilder MapInterviewerApplicationDeleteEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    await mediator.Send(new DeleteInterviewerApplicationCommand(id));
                    return Results.NoContent();
                })
                .WithName(nameof(DeleteInterviewerApplicationEndpoint))
                .WithSummary("Deletes Interviewer Application by id")
                .WithDescription("Deletes Interviewer Application by id")
                .Produces(StatusCodes.Status204NoContent)
                .RequirePermission("Permissions.Interviewer.Delete")
                .MapToApiVersion(1);
        }
    }
}
