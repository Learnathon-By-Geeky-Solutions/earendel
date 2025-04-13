using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Get; 

namespace TalentMesh.Module.Quizzes.Api.Endpoints 
{
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
                .WithTags("QuizAttempts")
                .WithName("GetQuizQuestionForAttempt")
                .Produces<QuizQuestionDto>(StatusCodes.Status200OK) 
                .Produces<object>(StatusCodes.Status200OK) 
                .Produces(StatusCodes.Status404NotFound)   
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}