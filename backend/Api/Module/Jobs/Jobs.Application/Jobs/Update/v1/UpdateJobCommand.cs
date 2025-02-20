using MediatR;

namespace TalentMesh.Module.Job.Application.Jobs.Update.v1;
public sealed record UpdateJobCommand(
    Guid Id,
    string? Name,
    string? Description = null) : IRequest<UpdateJobResponse>;
