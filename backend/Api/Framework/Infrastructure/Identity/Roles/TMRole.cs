using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Roles;
[ExcludeFromCodeCoverage]

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
