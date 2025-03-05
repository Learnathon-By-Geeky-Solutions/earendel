using MediatR;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Update.v1;
public sealed record UpdateSeniorityLevelJunctionCommand(
    Guid Id,
    Guid SeniorityLevelId,
    Guid SkillId) : IRequest<UpdateSeniorityLevelJunctionResponse>;