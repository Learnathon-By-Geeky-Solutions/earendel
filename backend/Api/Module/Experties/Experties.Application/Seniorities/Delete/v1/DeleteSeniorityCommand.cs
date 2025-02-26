using MediatR;

namespace TalentMesh.Module.Experties.Application.Seniorities.Delete.v1;
public sealed record DeleteSeniorityCommand(
    Guid Id) : IRequest;
