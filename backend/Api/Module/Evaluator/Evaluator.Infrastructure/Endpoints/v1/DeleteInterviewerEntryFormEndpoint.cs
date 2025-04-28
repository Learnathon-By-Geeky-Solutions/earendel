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
    public static class DeleteInterviewerEntryFormEndpoint
    {
        internal static RouteHandlerBuilder MapInterviewerEntryFormDeleteEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    await mediator.Send(new DeleteInterviewerEntryFormCommand(id));
                    return Results.NoContent();
                })
                .WithName(nameof(DeleteInterviewerEntryFormEndpoint))
                .WithSummary("Deletes Interviewer Entry Form by id")
                .WithDescription("Deletes Interviewer Entry Form by id")
                .Produces(StatusCodes.Status204NoContent)
                .RequirePermission("Permissions.InterviewerEntryForms.Delete")
                .MapToApiVersion(1);
        }
    }
}
