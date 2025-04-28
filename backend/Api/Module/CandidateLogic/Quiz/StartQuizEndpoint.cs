using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // For [FromBody]
using Microsoft.AspNetCore.Routing;
using System;
using TalentMesh.Framework.Infrastructure.Auth.Policy;

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
            return app.MapPost("/quiz-attempts/start",
                async (
                    IMediator mediator,
                    [FromBody] StartQuizRequest request 
                                                        
                    ) =>
                {
                    var command = new Application.QuizAttempts.Start.StartQuizCommand(request.UserId);
                    return await mediator.Send(command);
                })
                .WithTags("QuizView")
                .WithName("StartQuizAttempt")
                .RequirePermission("Permissions.QuizAttempts.Create")   // ← submitting is an update on the attempt
                .Produces<object>(StatusCodes.Status200OK, contentType: "application/json") // Returns { AttemptId: "guid" }
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}