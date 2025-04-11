using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.SubmitAnswer // Or your preferred namespace
{
    // Command to submit an answer for a specific question in an attempt
    public record SubmitAnswerCommand(
        Guid AttemptId,
        Guid QuestionId,
        int SelectedOption // The option number selected by the user (1-4)
    ) : IRequest<IResult>; // Returns correctness and potentially updated score
}