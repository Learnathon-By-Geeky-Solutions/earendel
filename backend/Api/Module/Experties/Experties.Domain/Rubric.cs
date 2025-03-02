using System.Net.Http.Headers;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Experties.Domain.Events;


namespace TalentMesh.Module.Experties.Domain;
public class Rubric : AuditableEntity, IAggregateRoot
{
    public string Title { get; private set; } = default!;
    public string? RubricDescription { get; private set; }
    public Guid? SubSkillId { get; private set; }
    public Guid? SeniorityLevelId { get; private set; }
    public decimal Weight { get; private set; }
    public virtual Seniority Seniority { get; private set; } = default!;
    public virtual SubSkill SubSkill { get; private set; } = default!;

    public static Rubric Create(string title, string rubricDescription, Guid? subSkillId, Guid? seniorityLevelId, decimal weight)
    {
        var Rubric = new Rubric
        {
            Title = title,
            RubricDescription = rubricDescription,
            SubSkillId = subSkillId,
            SeniorityLevelId = seniorityLevelId,
            Weight = weight

        };

        Rubric.QueueDomainEvent(new RubricCreated() { Rubric = Rubric });

        return Rubric;
    }

    public Rubric Update(string? title, string? rubricDescription, Guid? subSkillId, Guid? seniorityLevelId, decimal? weight)
    {
        if (title is not null && Title?.Equals(title, StringComparison.OrdinalIgnoreCase) is not true) Title = title;
        if (rubricDescription is not null && RubricDescription?.Equals(rubricDescription, StringComparison.OrdinalIgnoreCase) is not true) RubricDescription = rubricDescription;
        if (subSkillId.HasValue && subSkillId.Value is not null && subSkillId != subSkillId.Value)
        {
            SubSkillId = subSkillId.Value;
        }
        if (seniorityLevelId.HasValue && seniorityLevelId.Value is not null && seniorityLevelId != seniorityLevelId.Value)
        {
            SeniorityLevelId = seniorityLevelId.Value;
        }
        if (weight.HasValue && weight.Value > 0 && Weight != weight.Value)
            Weight = weight.Value;
        this.QueueDomainEvent(new RubricUpdated() { Rubric = this });

        return this;
    }

    public static Rubric Update(Guid id, string title, string rubricDescription, Guid subSkillId, Guid? seniorityLevelId, decimal weight)
    {
        var Rubric = new Rubric
        {
            Id = id,
            Title = title,
            RubricDescription = rubricDescription,
            SubSkillId = subSkillId,
            SeniorityLevelId = seniorityLevelId,
            Weight = weight
        };

        Rubric.QueueDomainEvent(new RubricUpdated() { Rubric = Rubric });

        return Rubric;
    }
}

