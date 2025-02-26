using MediatR;

namespace TalentMesh.Module.Job.Application.JobApplication.Delete.v1;
public sealed record DeleteJobApplicationCommand(
    Guid Id) : IRequest;
