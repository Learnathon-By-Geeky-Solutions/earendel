using MediatR;

namespace TalentMesh.Module.Experties.Application.Skills.Update.v1;
public sealed record UpdateSkillCommand(
    Guid Id,
    string? Name,
    string? Description = null) : IRequest<UpdateSkillResponse>;
