using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Delete.v1;
public sealed record DeleteInterviewFeedbackCommand(
    Guid Id) : IRequest;
