using System.Security.Claims;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Users.Abstractions;
public interface ICurrentUserInitializer
{
    void SetCurrentUser(ClaimsPrincipal user);

    void SetCurrentUserId(string userId);
}
