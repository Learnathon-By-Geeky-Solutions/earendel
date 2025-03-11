namespace TalentMesh.Framework.Core.Identity.Users.Dtos;
public class UserDetail
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }
    public string? Email { get; set; }

    public bool IsActive { get; set; } = true;

    public bool EmailConfirmed { get; set; }
    public Uri? ImageUrl { get; set; }
}
