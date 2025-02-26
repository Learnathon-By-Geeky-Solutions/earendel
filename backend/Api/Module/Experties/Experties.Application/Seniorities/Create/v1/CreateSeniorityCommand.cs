using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Seniorities.Create.v1;
public sealed record CreateSeniorityCommand(
    [property: DefaultValue("Internship")] string? Name,
    [property: DefaultValue("0 Years of Experience")] string? Description = null) : IRequest<CreateSeniorityResponse>;

