using MediatR;

namespace TalentMesh.Module.Interviews.Application.Interviews.Delete.v1;
public sealed record DeleteInterviewCommand(
    Guid Id) : IRequest;
