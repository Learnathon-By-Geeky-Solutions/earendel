using TalentMesh.Framework.Core.Storage.File.Features;
using MediatR;

namespace TalentMesh.Framework.Core.Identity.Users.Features.UpdateUser;
public class UpdateUserCommand : IRequest
{
    public string Id { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public FileUploadCommand? Image { get; set; }
    public bool DeleteCurrentImage { get; set; }
}
