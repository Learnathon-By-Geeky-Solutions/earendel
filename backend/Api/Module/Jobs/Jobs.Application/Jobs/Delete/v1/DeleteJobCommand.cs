using MediatR;

namespace TalentMesh.Module.Job.Application.Jobs.Delete.v1;
public sealed record DeleteJobCommand(
    Guid Id) : IRequest;
