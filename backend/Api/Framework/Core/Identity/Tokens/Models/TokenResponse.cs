namespace TalentMesh.Framework.Core.Identity.Tokens.Models;
public class TokenResponse
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public List<string> Roles { get; set; }  // Add roles property

    public TokenResponse(string userId, string token, string refreshToken, DateTime refreshTokenExpiryTime, List<string> roles)
    {
        UserId = userId;
        Token = token;
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
        Roles = roles;
    }
}