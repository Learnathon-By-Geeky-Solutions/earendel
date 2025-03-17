using System.Net.Http.Headers;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Experties.Domain.Events;


namespace TalentMesh.Module.Experties.Domain;
public class Skill : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    public static Skill Create(string name, string? description)
    {
        var skill = new Skill
        {
            Name = name,
            Description = description
        };

        skill.QueueDomainEvent(new SkillCreated() { Skill = skill });

        return skill;
    }

    public Skill Update(string? name, string? description)
    {
        if (name is not null && Name?.Equals(name, StringComparison.OrdinalIgnoreCase) is not true) Name = name;
        if (description is not null && Description?.Equals(description, StringComparison.OrdinalIgnoreCase) is not true) Description = description;

        this.QueueDomainEvent(new SkillUpdated() { Skill = this });

        return this;
    }

    public static Skill Update(Guid id, string name, string? description)
    {
        var skill = new Skill
        {
            Id = id,
            Name = name,
            Description = description
        };

        skill.QueueDomainEvent(new SkillUpdated() { Skill = skill });

        return skill;
    }
}

