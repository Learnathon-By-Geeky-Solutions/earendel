using System.Net.Http.Headers;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Experties.Domain.Events;


namespace TalentMesh.Module.Experties.Domain;
public class Seniority : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    
    public static Seniority Create(string name, string? description)
    {
        var seniority = new Seniority
        {
            Name = name,
            Description = description
        };

        seniority.QueueDomainEvent(new SeniorityCreated() { Seniority = seniority });

        return seniority;
    }
    public Seniority Update(string? name, string? description)
    {
        if (name is not null && Name?.Equals(name, StringComparison.OrdinalIgnoreCase) is not true)
        Name = name;
        if (description is not null && Description?.Equals(description, StringComparison.OrdinalIgnoreCase) is not true) 
        Description = description;

        this.QueueDomainEvent(new SeniorityUpdated() { Seniority = this });

        return this;
    }
}

