using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Create.v1;
public sealed record CreateSeniorityLevelJunctionCommand(
    Guid SeniorityLevelId,
    Guid SkillId
) : IRequest<CreateSeniorityLevelJunctionResponse>;

