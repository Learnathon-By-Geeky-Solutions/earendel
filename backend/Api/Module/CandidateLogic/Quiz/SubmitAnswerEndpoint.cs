﻿using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.SubmitAnswer; // Namespace for Command
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Api.Endpoints // Or your preferred namespace
{
    [ExcludeFromCodeCoverage]
    // Request body for submitting an answer
    public class SubmitAnswerRequest
    {
        public Guid QuestionId { get; set; }
        public int SelectedOption { get; set; } // e.g., 1, 2, 3, or 4
    }
    [ExcludeFromCodeCoverage]
    public static class SubmitAnswerEndpoint
    {
        public static RouteHandlerBuilder MapSubmitAnswerEndpoint(this IEndpointRouteBuilder app)
        {
            // POST endpoint to submit an answer
            return app.MapPost("/quiz-attempts/{attemptId:guid}/submit",
                async (
                    Guid attemptId, // Get attemptId from route
                    [FromBody] SubmitAnswerRequest request, // Get answer details from body
                    IMediator mediator) =>
                {
                    var command = new SubmitAnswerCommand(
                        AttemptId: attemptId,
                        QuestionId: request.QuestionId,
                        SelectedOption: request.SelectedOption
                    );
                    return await mediator.Send(command);
                })
                .WithTags("QuizView")
                .WithName("SubmitQuizAnswer")
                .RequirePermission("Permissions.QuizAttemptAnswers.Update")   // ← submitting is an update on the attempt
                .Produces<object>(StatusCodes.Status200OK) // Returns { IsCorrect: bool, NewScore: decimal }
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound) // If attempt or question not found
                .Produces(StatusCodes.Status409Conflict) // If already answered
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}