using MediatR;

namespace TalentMesh.Module.Notifications.Application.Notifications.Update.v1;

public sealed record UpdateNotificationCommand(
    Guid Id,
    Guid UserId,          // Added UserId as it's part of Notification
    string? Entity,       // Updated with Entity (as in the fields)
    string? EntityType,   // Updated with EntityType (as in the fields)
    string? Message = null // Updated with Message (as in the fields)
) : IRequest<UpdateNotificationResponse>;
