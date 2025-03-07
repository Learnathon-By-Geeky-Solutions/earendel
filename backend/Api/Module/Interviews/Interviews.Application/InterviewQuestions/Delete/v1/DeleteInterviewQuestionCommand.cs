using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Delete.v1;
public sealed record DeleteInterviewQuestionCommand(
    Guid Id) : IRequest;
