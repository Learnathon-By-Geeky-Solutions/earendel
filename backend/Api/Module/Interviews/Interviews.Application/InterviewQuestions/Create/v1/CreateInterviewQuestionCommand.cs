using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Create.v1;

public sealed record CreateInterviewQuestionCommand(
    Guid RubricId,
    string QuestionText
) : IRequest<CreateInterviewQuestionResponse>;
