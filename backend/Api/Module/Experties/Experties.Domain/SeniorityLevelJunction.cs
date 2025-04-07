using System;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Experties.Domain.Events;

namespace TalentMesh.Module.Experties.Domain;
public class SeniorityLevelJunction : AuditableEntity, IAggregateRoot
{
    public Guid SkillId { get; private set; }
    public virtual Skill Skill { get; private set; } = default!;
    public Guid SeniorityLevelId { get; private set; }
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

}