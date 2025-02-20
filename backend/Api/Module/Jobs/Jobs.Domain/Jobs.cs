using System.Net.Http.Headers;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Job.Domain.Events;


namespace TalentMesh.Module.Job.Domain;
public class Jobs : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    
    public static Jobs Create(string name, string? description)
    {
        var user = new Jobs
        {
            Name = name,
            Description = description
        };

        user.QueueDomainEvent(new JobCreated() { User = user });

        return user;
    }

    public Jobs Update(string? name, string? description)
    {
        if (name is not null && Name?.Equals(name, StringComparison.OrdinalIgnoreCase) is not true) Name = name;
        if (description is not null && Description?.Equals(description, StringComparison.OrdinalIgnoreCase) is not true) Description = description;

        this.QueueDomainEvent(new JobUpdated() { User = this });

        return this;
    }

    public static Jobs Update(Guid id, string name, string? description)
    {
        var user = new Jobs
        {
            Id = id,
            Name = name,
            Description = description
        };

        user.QueueDomainEvent(new JobUpdated() { User = user });

        return user;
    }
}

