using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.User.Domain.Events;


namespace TalentMesh.Module.User.Domain;
public class User : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    public static User Create(string name, string? description)
    {
        var user = new User
        {
            Name = name,
            Description = description
        };

        user.QueueDomainEvent(new UserCreated() { User = user });

        return user;
    }

    public User Update(string? name, string? description)
    {
        if (name is not null && Name?.Equals(name, StringComparison.OrdinalIgnoreCase) is not true) Name = name;
        if (description is not null && Description?.Equals(description, StringComparison.OrdinalIgnoreCase) is not true) Description = description;

        this.QueueDomainEvent(new UserUpdated() { User = this });

        return this;
    }

    public static User Update(Guid id, string name, string? description)
    {
        var user = new User
        {
            Id = id,
            Name = name,
            Description = description
        };

        user.QueueDomainEvent(new UserUpdated() { User = user });

        return user;
    }
}

