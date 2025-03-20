using TalentMesh.Framework.Core.Identity.Users.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Users.Features.AssignUserRole;
[ExcludeFromCodeCoverage]
public class AssignUserRoleCommand
{
    public List<UserRoleDetail> UserRoles { get; set; } = new();
}
