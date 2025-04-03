using System;
using System.Collections.Generic;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Update.v1
{
    public sealed record UpdateSeniorityLevelJunctionResponse(List<Guid> JunctionIds);
}
