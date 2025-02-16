using TalentMesh.Framework.Core.Identity.Tokens.Features.Generate;
using TalentMesh.Framework.Core.Identity.Tokens.Features.Refresh;
using TalentMesh.Framework.Core.Identity.Tokens.Models;

namespace FSH.Framework.Core.Identity.Tokens;
public interface ITokenService
{
    Task<TokenResponse> GenerateTokenAsync(TokenGenerationCommand request, string ipAddress, CancellationToken cancellationToken);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenCommand request, string ipAddress, CancellationToken cancellationToken);

}
