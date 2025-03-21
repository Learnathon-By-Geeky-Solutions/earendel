using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Users.Features.GoogleLogin
{
    [ExcludeFromCodeCoverage]
    // Updated RegisterUserResponse to include Token and RefreshToken
    public record GoogleLoginUserResponse(string UserId, string Token, string RefreshToken, List<string> Roles);
}
