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
    public static class GetInterviewerApplicationEndpoint
    {
        internal static RouteHandlerBuilder MapGetInterviewerApplicationEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    var response = await mediator.Send(new GetInterviewerApplicationRequest(id));
                    return Results.Ok(response);
                })
                .WithName(nameof(GetInterviewerApplicationEndpoint))
                .WithSummary("Gets Interviewer Application by id")
                .WithDescription("Retrieves an Interviewer Application by its identifier")
                .Produces<InterviewerApplicationResponse>()
                .RequirePermission("Permissions.InterviewerApplications.View")
                .MapToApiVersion(1);
        }
    }
}
