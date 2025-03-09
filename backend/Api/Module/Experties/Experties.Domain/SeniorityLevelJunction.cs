using System;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Experties.Domain.Events;

namespace TalentMesh.Module.Experties.Domain;
public class SeniorityLevelJunction : AuditableEntity, IAggregateRoot
{
    public Guid SeniorityLevelId { get; private set; }
    public Guid SkillId { get; private set; }

    public virtual Skill Skill { get; private set; } = default!;

    public virtual Seniority Seniority { get; private set; } = default!;


    public static SeniorityLevelJunction Create(Guid seniorityLevelId, Guid skillId)
    {
        var junction = new SeniorityLevelJunction
        {
            SeniorityLevelId = seniorityLevelId,
            SkillId = skillId
        };

        junction.QueueDomainEvent(new SeniorityLevelJunctionCreated() { SeniorityLevelJunction = junction });

        return junction;
    }

    public SeniorityLevelJunction Update(Guid seniorityLevelId, Guid skillId)
    {
        if (seniorityLevelId != Guid.Empty && !SeniorityLevelId.Equals(seniorityLevelId))
            SeniorityLevelId = seniorityLevelId;

        if (skillId != Guid.Empty && !SkillId.Equals(skillId))
            SkillId = skillId;

        this.QueueDomainEvent(new SeniorityLevelJunctionUpdated() { SeniorityLevelJunction = this });

        return this;
    }

    public static SeniorityLevelJunction Update(Guid id, Guid seniorityLevelId, Guid skillId)
    {
        var junction = new SeniorityLevelJunction
        {
            Id = id,
            SeniorityLevelId = seniorityLevelId,
            SkillId = skillId
        };

        junction.QueueDomainEvent(new SeniorityLevelJunctionUpdated() { SeniorityLevelJunction = junction });

        return junction;
    }
}