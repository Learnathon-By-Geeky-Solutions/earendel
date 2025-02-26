using MediatR;

namespace TalentMesh.Module.Experties.Application.Rubrics.Update.v1;
public sealed record UpdateRubricCommand(
    Guid Id,
    Guid SubSkillId,
    Guid SeniorityLevelId,
    decimal Weight,
    string? Title,
    string? RubricDescription = null) : IRequest<UpdateRubricResponse>;
