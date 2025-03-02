using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Create.v1;

public sealed record CreateQuizQuestionCommand(
    [property: DefaultValue(1)] int CorrectOption,
    [property: DefaultValue("Sample Question")] string QuestionText = null!,
    [property: DefaultValue("Option 1")] string Option1 = null!,
    [property: DefaultValue("Option 2")] string Option2 = null!,
    [property: DefaultValue("Option 3")] string Option3 = null!,
    [property: DefaultValue("Option 4")] string Option4 = null!
) : IRequest<CreateQuizQuestionResponse>;
