using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Experties.Application.SubSkills.Create.v1;

public sealed record CreateSubSkillCommand(
    Guid SkillId,
    [property: DefaultValue("Sample User")] string? Name = null,
    [property: DefaultValue("Descriptive Description")] string? Description = null
) : IRequest<CreateSubSkillResponse>;
