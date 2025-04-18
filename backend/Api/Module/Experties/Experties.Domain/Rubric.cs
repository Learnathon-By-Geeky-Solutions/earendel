using System.Net.Http.Headers;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Experties.Domain.Events;

namespace TalentMesh.Module.Experties.Domain
{
    public class Rubric : AuditableEntity, IAggregateRoot
    {
        public string Title { get; private set; } = default!;
        public string? RubricDescription { get; private set; }
        public Guid? SubSkillId { get; private set; }
        public Guid? SeniorityId { get; private set; }
        public decimal Weight { get; private set; }
        public virtual Seniority Seniority { get; private set; } = default!;
        public virtual SubSkill SubSkill { get; private set; } = default!;

        public static Rubric Create(string title, string rubricDescription, Guid? subSkillId, Guid? seniorityLevelId, decimal weight)
        {
            var rubric = new Rubric
            {
                Title = title,
                RubricDescription = rubricDescription,
                SubSkillId = subSkillId,
                SeniorityId = seniorityLevelId,
                Weight = weight
            };

            rubric.QueueDomainEvent(new RubricCreated() { Rubric = rubric });

            return rubric;
        }

        public Rubric Update(string? title, string? rubricDescription, Guid? subSkillId, Guid? seniorityLevelId, decimal? weight)
        {
            if (title is not null && Title?.Equals(title, StringComparison.OrdinalIgnoreCase) is not true)
                Title = title;

            if (rubricDescription is not null && RubricDescription?.Equals(rubricDescription, StringComparison.OrdinalIgnoreCase) is not true)
                RubricDescription = rubricDescription;

            if (subSkillId is not null && subSkillId.HasValue)
            {
                SubSkillId = subSkillId.Value;
            }

            if (seniorityLevelId is not null && seniorityLevelId.HasValue)
            {
                SeniorityId = seniorityLevelId.Value;
            }

            if (weight is not null && weight.HasValue && weight.Value > 0 && Weight != weight.Value)
                Weight = weight.Value;

            this.QueueDomainEvent(new RubricUpdated() { Rubric = this });

            return this;
        }

    }
}
