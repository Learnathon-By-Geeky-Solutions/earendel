using System;
using System.Collections.Generic;
using MediatR;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Update.v1
{
    public sealed record UpdateSeniorityLevelJunctionCommand(
        Guid SkillId,
        List<Guid> SeniorityLevelIds
    ) : IRequest<UpdateSeniorityLevelJunctionResponse>;
}
