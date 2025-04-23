using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Skills.Create.v1;

public sealed record CreateSkillCommand(
    [property: DefaultValue("Sample User")] string Name,
    [property: DefaultValue("Descriptive Description")] string Description,
    List<Guid> SeniorityLevels) : IRequest<CreateSkillResponse>
{
    public static readonly Guid[] SeniorityLevelDefaults =
    {
        new("00000000-0000-0000-0000-000000000001"),
        new("00000000-0000-0000-0000-000000000002")
    };
}
