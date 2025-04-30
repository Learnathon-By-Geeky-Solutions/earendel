using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Get;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Api.Endpoints 
{
    [ExcludeFromCodeCoverage]
    public static class GetQuizQuestionEndpoint
    {
        public static RouteHandlerBuilder MapGetQuizQuestionEndpoint(this IEndpointRouteBuilder app)
        {
            // GET endpoint to retrieve a question for an attempt
            return app.MapGet("/quiz-attempts/{attemptId:guid}/question",
                async (
                    Guid attemptId, // Get attemptId from route
                    IMediator mediator) =>
                {
                    var query = new GetRandomQuizQuestionQuery(attemptId);
                    return await mediator.Send(query);
                })
                .WithTags("QuizView")
                .WithName("GetQuizQuestionForAttempt")
                .RequirePermission("Permissions.QuizQuestions.View")   // ← only view needed
                .Produces<QuizQuestionDto>(StatusCodes.Status200OK) 
                .Produces<object>(StatusCodes.Status200OK) 
                .Produces(StatusCodes.Status404NotFound)   
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}