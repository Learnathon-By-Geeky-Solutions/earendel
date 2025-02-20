using Microsoft.AspNetCore.Identity;

namespace TalentMesh.Framework.Infrastructure.Identity.RoleClaims;
public class TMRoleClaim : IdentityRoleClaim<string>
{
    public string? CreatedBy { get; init; }
    public DateTimeOffset CreatedOn { get; init; }
}
