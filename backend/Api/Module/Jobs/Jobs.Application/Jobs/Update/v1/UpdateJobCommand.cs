using MediatR;

namespace TalentMesh.Module.Job.Application.Jobs.Update.v1;
public sealed record UpdateJobCommand(
    Guid Id,
    string? Name,
    string? Description = null,
    string? Requirments = null,
    string? Location = null,
    string? JobType = null,
    string? ExperienceLevel=null,
    string? Salary = null
    ) : IRequest<UpdateJobResponse>;
