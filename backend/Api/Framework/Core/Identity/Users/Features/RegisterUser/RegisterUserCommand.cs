using System.Text.Json.Serialization;
using MediatR;
using TalentMesh.Framework.Core.Identity.Users.Dtos;

namespace TalentMesh.Framework.Core.Identity.Users.Features.RegisterUser;
public class RegisterUserCommand : IRequest<RegisterUserResponse>
{
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public UserRole Role { get; set; }

    [JsonIgnore]
    public string? Origin { get; set; }
}
