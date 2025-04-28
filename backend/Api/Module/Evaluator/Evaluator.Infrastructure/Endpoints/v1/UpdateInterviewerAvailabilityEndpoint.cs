using TalentMesh.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]
    public static class UpdateInterviewerAvailabilityEndpoint
    {
        internal static RouteHandlerBuilder MapInterviewerAvailabilityUpdateEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPut("/{id:guid}", async (Guid id, UpdateInterviewerAvailabilityCommand request, ISender mediator) =>
                {
                    if (id != request.Id)
                        return Results.BadRequest();

                    var response = await mediator.Send(request);
                    return Results.Ok(response);
                })
                .WithName(nameof(UpdateInterviewerAvailabilityEndpoint))
                .WithSummary("Update Interviewer Availability")
                .WithDescription("Updates Interviewer Availability by its identifier")
                .Produces<UpdateInterviewerAvailabilityResponse>()
                .RequirePermission("Permissions.InterviewerAvailabilities.Update")
                .MapToApiVersion(1);
        }
    }
}
