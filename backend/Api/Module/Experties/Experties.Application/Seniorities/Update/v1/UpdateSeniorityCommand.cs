using MediatR;

namespace TalentMesh.Module.Experties.Application.Seniorities.Update.v1;
public sealed record UpdateSeniorityCommand(
    Guid Id,
    string? Name,
    string? Description = null) : IRequest<UpdateSeniorityResponse>;
