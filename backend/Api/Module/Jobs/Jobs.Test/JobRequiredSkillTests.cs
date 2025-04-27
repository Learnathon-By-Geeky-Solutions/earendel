using System;
using System.Linq;
using Xunit;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Events;
using TalentMesh.Framework.Core.Domain;

namespace TalentMesh.Module.Job.Tests
{
    public class JobRequiredSkillTests
    {
        [Fact]
        public void Create_SetsJobAndSkillIds_AndQueuesCreatedEvent()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var skillId = Guid.NewGuid();

            // Act
            var reqSkill = JobRequiredSkill.Create(jobId, skillId);

            // Assert: properties
            Assert.Equal(jobId, reqSkill.JobId);
            Assert.Equal(skillId, reqSkill.SkillId);

            // Assert: one domain event of the correct type
            var events = ((AuditableEntity)reqSkill).DomainEvents;
            Assert.Single(events);
            var createdEvt = Assert.IsType<JobRequiredSkillCreated>(events.Single());
            Assert.Same(reqSkill, createdEvt.JobRequiredSkill);
        }

        [Fact]
        public void Update_WithDifferentIds_UpdatesProperties_AndQueuesUpdatedEvent()
        {
            // Arrange
            var originalJobId = Guid.NewGuid();
            var originalSkillId = Guid.NewGuid();
            var reqSkill = JobRequiredSkill.Create(originalJobId, originalSkillId);

            var beforeCount = ((AuditableEntity)reqSkill).DomainEvents.Count;

            var newJobId = Guid.NewGuid();
            var newSkillId = Guid.NewGuid();

            // Act
            var updated = reqSkill.Update(newJobId, newSkillId);

            // Assert: updated properties
            Assert.Equal(newJobId, updated.JobId);
            Assert.Equal(newSkillId, updated.SkillId);
            Assert.Same(reqSkill, updated);

            // Assert: one new event appended
            var events = ((AuditableEntity)reqSkill).DomainEvents;
            Assert.Equal(beforeCount + 1, events.Count);
            var updatedEvt = events.Last() as JobRequiredSkillUpdated;
            Assert.NotNull(updatedEvt);
            Assert.Same(reqSkill, updatedEvt!.JobRequiredSkill);
        }

        [Fact]
        public void Update_WithSameIds_DoesNotChangeProperties_ButStillQueuesUpdatedEvent()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var skillId = Guid.NewGuid();
            var reqSkill = JobRequiredSkill.Create(jobId, skillId);

            var beforeCount = ((AuditableEntity)reqSkill).DomainEvents.Count;

            // Act
            reqSkill.Update(jobId, skillId);

            // Assert: properties unchanged
            Assert.Equal(jobId, reqSkill.JobId);
            Assert.Equal(skillId, reqSkill.SkillId);

            // Assert: one new event appended
            var events = ((AuditableEntity)reqSkill).DomainEvents;
            Assert.Equal(beforeCount + 1, events.Count);
            Assert.IsType<JobRequiredSkillUpdated>(events.Last());
        }
    }
}
