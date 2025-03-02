using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Create.v1;

public sealed record CreateQuizAttemptAnswerCommand(
     Guid AttemptId,
     Guid QuestionId,
    [property: DefaultValue(1)] int SelectedOption,
    [property: DefaultValue(false)] bool IsCorrect
) : IRequest<CreateQuizAttemptAnswerResponse>;