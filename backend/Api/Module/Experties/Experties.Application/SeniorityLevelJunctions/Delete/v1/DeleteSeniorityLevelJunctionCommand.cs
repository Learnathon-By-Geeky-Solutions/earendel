using MediatR;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Delete.v1;
public sealed record DeleteSeniorityLevelJunctionCommand(
    Guid Id) : IRequest;

