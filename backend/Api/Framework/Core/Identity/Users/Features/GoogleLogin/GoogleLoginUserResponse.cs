namespace TalentMesh.Framework.Core.Identity.Users.Features.GoogleLogin
{
    // Updated RegisterUserResponse to include Token and RefreshToken
    public record GoogleLoginUserResponse(string UserId, string Token, string RefreshToken, List<string> Roles);
}
