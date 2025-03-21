using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Users.Dtos;
[ExcludeFromCodeCoverage]
public class UserRoleDetail
{
    public string? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? Description { get; set; }
    public bool Enabled { get; set; }
}
