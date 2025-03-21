using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Roles.Features.UpdatePermissions;
[ExcludeFromCodeCoverage]
public class UpdatePermissionsCommand
{
    public string RoleId { get; set; } = default!;
    public List<string> Permissions { get; set; } = default!;
}
