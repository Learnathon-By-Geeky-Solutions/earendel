using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class UpdateInterviewFeedbackEndpoint
{
    internal static RouteHandlerBuilder MapInterviewFeedbackUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateInterviewFeedbackCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateInterviewFeedbackEndpoint))
            .WithSummary("update Interview")
            .WithDescription("update Interview")
            .Produces<UpdateInterviewFeedbackResponse>()
            // .RequirePermission("Permissions.Products.Update")
            .MapToApiVersion(1);
    }
}
