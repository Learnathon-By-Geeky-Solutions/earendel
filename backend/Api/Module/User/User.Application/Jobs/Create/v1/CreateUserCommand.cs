using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Job.Application.Jobs.Create.v1;
public sealed record CreateUserCommand(
    [property: DefaultValue("Sample User")] string? Name,
    [property: DefaultValue("Descriptive Description")] string? Description = null) : IRequest<CreateUserResponse>;

