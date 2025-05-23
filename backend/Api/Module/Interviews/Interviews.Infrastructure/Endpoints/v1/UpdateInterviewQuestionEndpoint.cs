using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class UpdateInterviewQuestionEndpoint
{
    internal static RouteHandlerBuilder MapInterviewQuestionUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateInterviewQuestionCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateInterviewQuestionEndpoint))
            .WithSummary("update InterviewQuestion")
            .WithDescription("update InterviewQuestion")
            .Produces<UpdateInterviewQuestionResponse>()
            .RequirePermission("Permissions.InterviewQuestions.Update") 
            .MapToApiVersion(1);
    }
}
