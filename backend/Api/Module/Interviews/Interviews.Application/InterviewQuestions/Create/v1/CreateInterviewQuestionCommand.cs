using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Create.v1;

public sealed record CreateInterviewQuestionCommand(
    Guid RubricId,
    Guid InterviewId,
    string QuestionText
) : IRequest<CreateInterviewQuestionResponse>;
