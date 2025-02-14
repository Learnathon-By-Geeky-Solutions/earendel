namespace TalentMesh.Module.User.Application.Users.Get.v1;
public sealed record UserResponse(Guid? Id, string Name, string? Description);
