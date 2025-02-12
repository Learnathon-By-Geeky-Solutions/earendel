using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.User.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.User.Application.Users.Create.v1;
public sealed class CreateUserHandler(
    ILogger<CreateUserHandler> logger,
    [FromKeyedServices("user:users")] IRepository<User.Domain.User> repository)
    : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = User.Domain.User.Create(request.Name!, request.Description);
        await repository.AddAsync(user, cancellationToken);
        logger.LogInformation("user created {UserId}", user.Id);
        return new CreateUserResponse(user.Id);
    }
}
