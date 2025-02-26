using MediatR;

namespace TalentMesh.Module.Experties.Application.Rubrics.Delete.v1;
public sealed record DeleteRubricCommand(
    Guid Id) : IRequest;
