using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Job.Application.Jobs.Create.v1;
public sealed record CreateJobCommand(
    [property: DefaultValue("Sample Job")] string? Name,
    [property: DefaultValue("Descriptive Description")] string? Description = null,
    [property: DefaultValue("Requirments")] string? Requirments = null,
    [property: DefaultValue("Location")] string? Location = null,
    [property: DefaultValue("JobType")] string? JobType = null,
    [property: DefaultValue("ExperienceLevel")] string? ExperienceLevel = null,
    [property: DefaultValue("Salarys")] string? Salary = null,
    [property: DefaultValue("PostedById")] Guid PostedById = default(Guid)
    ) : IRequest<CreateJobResponse>;

    

