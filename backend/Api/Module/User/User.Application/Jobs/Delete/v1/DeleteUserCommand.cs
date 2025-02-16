using MediatR;

namespace TalentMesh.Module.Job.Application.Jobs.Delete.v1;
public sealed record DeleteUserCommand(
    Guid Id) : IRequest;
