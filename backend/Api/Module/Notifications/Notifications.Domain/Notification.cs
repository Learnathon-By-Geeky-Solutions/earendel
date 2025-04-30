using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Notifications.Domain.Events;


namespace TalentMesh.Module.Notifications.Domain;
public class Notification : AuditableEntity, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public string? Entity { get; private set; }
    public string? EntityType { get; private set; }
    public string? Message { get; private set; }
    
    public static Notification Create(Guid userId, string? entity, string? entityType, string? message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Entity = entity,
            Message = message,
            EntityType = entityType,
        };

        notification.QueueDomainEvent(new NotificationCreated() { Notification = notification });

        return notification;
    }

    [ExcludeFromCodeCoverage]
    public Notification Update(Guid userId, string? entity, string? entityType, string? message)
    {
        if (userId != Guid.Empty && UserId != userId)
            UserId = userId;

        if (entity is not null && Entity?.Equals(entity, StringComparison.OrdinalIgnoreCase) is not true)
            Entity = entity;

        if (entityType is not null && EntityType?.Equals(entityType, StringComparison.OrdinalIgnoreCase) is not true)
            EntityType = entityType;

        if (message is not null && Message?.Equals(message, StringComparison.OrdinalIgnoreCase) is not true)
            Message = message;

        this.QueueDomainEvent(new NotificationUpdated() { Notification = this });

        return this;
    }

    [ExcludeFromCodeCoverage]
    public static Notification Update(Guid id, Guid userId, string? entity, string? entityType, string? message)
    {
        var notification = new Notification
        {
            Id = id,
            UserId = userId,
            Entity = entity,
            EntityType = entityType,
            Message = message
        };

        notification.QueueDomainEvent(new NotificationUpdated() { Notification = notification });

        return notification;
    }
}

