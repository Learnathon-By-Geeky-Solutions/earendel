using MediatR;

namespace TalentMesh.Module.Experties.Application.SubSkills.Update.v1;
public sealed record UpdateSubSkillCommand(
    Guid Id,
    Guid? SkillId = null,
    string? Name = null,
    string? Description = null) : IRequest<UpdateSubSkillResponse>;
