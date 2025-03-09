using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Create.v1;

public sealed record CreateQuizAttemptCommand(
    Guid UserId,
    [property: DefaultValue(0)] int TotalQuestions = 0,
    [property: DefaultValue(0.0)] decimal Score = 0.0m

) : IRequest<CreateQuizAttemptResponse>;
