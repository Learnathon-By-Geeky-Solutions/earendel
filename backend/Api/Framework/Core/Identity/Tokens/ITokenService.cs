using TalentMesh.Framework.Core.Identity.Tokens.Features.Generate;
using TalentMesh.Framework.Core.Identity.Tokens.Features.Refresh;
using TalentMesh.Framework.Core.Identity.Tokens.Models;
using System.Diagnostics.CodeAnalysis;
using TalentMesh.Framework.Core.Identity.Tokens.Features.Logout;

namespace TalentMesh.Framework.Core.Identity.Tokens;
public interface ITokenService
{
    Task<TokenResponse> GenerateTokenAsync(TokenGenerationCommand request, string ipAddress, CancellationToken cancellationToken);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenCommand request, string ipAddress, CancellationToken cancellationToken);
    Task<bool> LogoutAsync(LogoutCommand request, CancellationToken cancellationToken);
}
