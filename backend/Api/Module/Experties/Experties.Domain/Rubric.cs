using System;
using System.Diagnostics.CodeAnalysis;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Experties.Domain.Events;

namespace TalentMesh.Module.Experties.Domain
{
    [ExcludeFromCodeCoverage]
    public class Rubric : AuditableEntity, IAggregateRoot
    {
        public string Title { get; private set; } = default!;
        public string? RubricDescription { get; private set; }
        public Guid? SubSkillId { get; private set; }
        public Guid? SeniorityId { get; private set; }
        public decimal Weight { get; private set; }
        public virtual Seniority Seniority { get; private set; } = default!;
        public virtual SubSkill SubSkill { get; private set; } = default!;

        public static Rubric Create(
            string title,
            string? rubricDescription,
            Guid? subSkillId,
            Guid? seniorityId,
            decimal weight)
        {
            var rubric = new Rubric
            {
                Title = title,
                RubricDescription = rubricDescription,
                SubSkillId = subSkillId,
                SeniorityId = seniorityId,
                Weight = weight
            };
            rubric.QueueDomainEvent(new RubricCreated { Rubric = rubric });
            return rubric;
        }

        public Rubric Update(
            string? title,
            string? rubricDescription,
            Guid? subSkillId,
            Guid? seniorityId,
            decimal? weight)
        {
            Title = title ?? Title;
            RubricDescription = rubricDescription ?? RubricDescription;
            SubSkillId = subSkillId ?? SubSkillId;
            SeniorityId = seniorityId ?? SeniorityId;
            Weight = weight ?? Weight;

            QueueDomainEvent(new RubricUpdated { Rubric = this });
            return this;
        }
    }
}
