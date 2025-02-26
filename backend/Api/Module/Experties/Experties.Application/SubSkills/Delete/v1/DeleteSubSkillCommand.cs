using MediatR;

namespace TalentMesh.Module.Experties.Application.SubSkills.Delete.v1;
public sealed record DeleteSubSkillCommand(
    Guid Id) : IRequest;
