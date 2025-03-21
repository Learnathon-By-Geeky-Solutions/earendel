using TalentMesh.Framework.Infrastructure.Auth.Policy;

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Evaluator.Application.Interviewer.Get.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]
    public static class GetInterviewerEntryFormEndpoint
    {
        internal static RouteHandlerBuilder MapGetInterviewerEntryFormEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    var response = await mediator.Send(new GetInterviewerEntryFormRequest(id));
                    return Results.Ok(response);
                })
                .WithName(nameof(GetInterviewerEntryFormEndpoint))
                .WithSummary("Gets Interviewer Entry Form by id")
                .WithDescription("Retrieves an Interviewer Entry Form by its identifier")
                .Produces<InterviewerEntryFormResponse>()
                // .RequirePermission("Permissions.Interviews.View")
                .MapToApiVersion(1);
        }
    }
}
