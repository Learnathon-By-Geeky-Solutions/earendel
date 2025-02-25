using MediatR;

namespace TalentMesh.Module.Experties.Application.SubSkills.Update.v1;
public sealed record UpdateSubSkillCommand(
    Guid Id,
    Guid SkillId,
    string? Name,
    string? Description = null) : IRequest<UpdateSubSkillResponse>;
