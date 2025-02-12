using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.User.Application.Users.Create.v1;
public sealed record CreateUserCommand(
    [property: DefaultValue("Sample User")] string? Name,
    [property: DefaultValue("Descriptive Description")] string? Description = null) : IRequest<CreateUserResponse>;

