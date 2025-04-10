using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Skills.Update.v1
{
    public sealed record UpdateSkillCommand(
        Guid Id,
        string? Name,
        string? Description = null,
        List<Guid>? SeniorityLevelIds = null // Optional list of seniority levels
    ) : IRequest<UpdateSkillResponse>;
}
