using MediatR;

namespace TalentMesh.Module.Experties.Application.Skills.Delete.v1;
public sealed record DeleteSkillCommand(
    Guid Id) : IRequest;
