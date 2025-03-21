using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Users.Dtos;

[ExcludeFromCodeCoverage]
public class UserDetail
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public bool EmailConfirmed { get; set; }
    public Uri? ImageUrl { get; set; }
    public List<string> Roles { get; set; } = new();  // New property to store roles
}