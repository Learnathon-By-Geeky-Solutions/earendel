using System.Net.Http.Headers;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Experties.Domain.Events;


namespace TalentMesh.Module.Experties.Domain;
public class SubSkill : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public Guid? SkillId { get; private set; }
    public virtual Skill Skill { get; private set; } = default!;

    public static SubSkill Create(string name, string? description, Guid? skillId)
    {
        var subSkill = new SubSkill
        {
            Name = name,
            Description = description,
            SkillId = skillId
        };

        subSkill.QueueDomainEvent(new SubSkillCreated() { SubSkill = subSkill });

        return subSkill;
    }

    public SubSkill Update(string? name, string? description, Guid? skillId)
    {
        if (name is not null && Name?.Equals(name, StringComparison.OrdinalIgnoreCase) is not true) Name = name;
        if (description is not null && Description?.Equals(description, StringComparison.OrdinalIgnoreCase) is not true) Description = description;
        if (skillId.HasValue && skillId != SkillId)
        {
            SkillId = skillId.Value;
        }
        this.QueueDomainEvent(new SubSkillUpdated() { SubSkill = this });

        return this;
    }

    public static SubSkill Update(Guid id, string name, string? description, Guid? skillId)
    {
        var subSkill = new SubSkill
        {
            Id = id,
            Name = name,
            Description = description,
            SkillId = skillId
        };

        subSkill.QueueDomainEvent(new SubSkillUpdated() { SubSkill = subSkill });

        return subSkill;
    }
}

