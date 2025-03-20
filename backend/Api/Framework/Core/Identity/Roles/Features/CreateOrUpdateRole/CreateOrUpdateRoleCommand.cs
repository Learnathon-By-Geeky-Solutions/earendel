using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Roles.Features.CreateOrUpdateRole;

[ExcludeFromCodeCoverage]
public class CreateOrUpdateRoleCommand
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
