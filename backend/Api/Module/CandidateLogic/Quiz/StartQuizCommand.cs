using MediatR;
using Microsoft.AspNetCore.Http; // For IResult
using System;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Start // Or your preferred namespace
{
    // Command to start a new quiz attempt
    public record StartQuizCommand(
        Guid UserId // The ID of the user starting the quiz
    ) : IRequest<IResult>; // Returns the new AttemptId
}