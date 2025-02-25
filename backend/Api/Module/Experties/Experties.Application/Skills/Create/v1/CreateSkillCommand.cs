using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Skills.Create.v1;
public sealed record CreateSkillCommand(
    [property: DefaultValue("Sample User")] string? Name,
    [property: DefaultValue("Descriptive Description")] string? Description = null) : IRequest<CreateSkillResponse>;

