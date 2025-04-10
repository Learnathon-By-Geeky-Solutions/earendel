using System.ComponentModel;
using MediatR;
using System.Collections.Generic;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Create.v1;
public sealed record CreateSeniorityLevelJunctionCommand(
    Guid SkillId,
    List<Guid> SeniorityLevelIds
) : IRequest<CreateSeniorityLevelJunctionResponse>;
