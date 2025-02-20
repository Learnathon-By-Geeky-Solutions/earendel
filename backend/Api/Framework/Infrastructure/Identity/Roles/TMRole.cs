using Microsoft.AspNetCore.Identity;

namespace TalentMesh.Framework.Infrastructure.Identity.Roles;
public class TMRole : IdentityRole
{
    public string? Description { get; set; }

    public TMRole(string name, string? description = null)
        : base(name)
    {
        Description = description;
        NormalizedName = name.ToUpperInvariant();
    }
}
