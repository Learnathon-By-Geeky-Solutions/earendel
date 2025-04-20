using MediatR;

namespace TalentMesh.Module.Experties.Application.Rubrics.Update.v1;
public sealed record UpdateRubricCommand(
    Guid Id,
    Guid? SubSkillId = null,
    Guid? SeniorityId = null,
    decimal? Weight = null,
    string? Title = null,
    string? RubricDescription = null) : IRequest<UpdateRubricResponse>;
