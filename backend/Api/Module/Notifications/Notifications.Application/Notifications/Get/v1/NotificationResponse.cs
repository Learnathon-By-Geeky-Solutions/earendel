namespace TalentMesh.Module.Notifications.Application.Notifications.Get.v1;

public sealed record NotificationResponse(
    Guid? Id,
    Guid UserId,
    string? Entity,
    string? EntityType,
    string? Message
);
