using System.Text.Json.Serialization;
using MediatR;
using TalentMesh.Framework.Core.Identity.Users.Dtos;

namespace TalentMesh.Framework.Core.Identity.Users.Features.RegisterUser;
public class RegisterUserCommand : IRequest<RegisterUserResponse>
{
    // public string FirstName { get; set; } = default!;
    // public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    // public string? PhoneNumber { get; set; }
    public UserRole Role { get; set; } // Reference the UserRole enum

    [JsonIgnore]
    public string? Origin { get; set; }
}
