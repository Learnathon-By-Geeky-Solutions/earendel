using MediatR;

namespace TalentMesh.Module.User.Application.Users.Delete.v1;
public sealed record DeleteUserCommand(
    Guid Id) : IRequest;
