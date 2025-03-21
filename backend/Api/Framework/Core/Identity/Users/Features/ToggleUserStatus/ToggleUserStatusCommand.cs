using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Users.Features.ToggleUserStatus;
[ExcludeFromCodeCoverage]
public class ToggleUserStatusCommand
{
    public bool ActivateUser { get; set; }
    public string? UserId { get; set; }
}
