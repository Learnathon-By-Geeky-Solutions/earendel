using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class UpdateQuizQuestionEndpoint
{
    internal static RouteHandlerBuilder MapQuizQuestionUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateQuizQuestionCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateQuizQuestionEndpoint))
            .WithSummary("update a Question")
            .WithDescription("update a Question")
            .Produces<UpdateQuizQuestionResponse>()
            // .RequirePermission("Permissions.Products.Update")
            .MapToApiVersion(1);
    }
}
