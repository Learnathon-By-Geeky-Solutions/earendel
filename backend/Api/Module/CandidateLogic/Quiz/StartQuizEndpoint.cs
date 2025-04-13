using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // For [FromBody]
using Microsoft.AspNetCore.Routing;
using System;

namespace TalentMesh.Module.Quizzes.Api.Endpoints // Or your preferred namespace
{
    // Request body structure if UserId isn't from claims
    public class StartQuizRequest
    {
        public Guid UserId { get; set; }
    }

    public static class StartQuizEndpoint
    {
        public static RouteHandlerBuilder MapStartQuizEndpoint(this IEndpointRouteBuilder app)
        {
            // POST endpoint to start a quiz
            return app.MapPost("/quiz-attempts/start",
                async (
                    IMediator mediator,
                    [FromBody] StartQuizRequest request // Assuming UserId comes from body
                                                        // Alternatively, inject IHttpContextAccessor to get UserId from claims
                    ) =>
                {
                    // TODO: Add validation for request.UserId if needed
                    var command = new Application.QuizAttempts.Start.StartQuizCommand(request.UserId);
                    return await mediator.Send(command);
                })
                .WithTags("QuizAttempts")
                .WithName("StartQuizAttempt")
                .Produces<object>(StatusCodes.Status200OK, contentType: "application/json") // Returns { AttemptId: "guid" }
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}