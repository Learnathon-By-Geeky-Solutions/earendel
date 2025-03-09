using MediatR;

namespace TalentMesh.Module.Notifications.Application.Notifications.Delete.v1;
public sealed record DeleteNotificationCommand(
    Guid Id) : IRequest;
