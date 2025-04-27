using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Events;
using TalentMesh.Framework.Core.Domain;

namespace TalentMesh.Module.Job.Tests
{
    public class JobRequiredSubskillTests
    {
        [Fact]
        public void Create_SetsJobAndSubskillIds_AndQueuesCreatedEvent()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var subskillId = Guid.NewGuid();

            // Act
            var reqSub = JobRequiredSubskill.Create(jobId, subskillId);

            // Assert: properties
            Assert.Equal(jobId, reqSub.JobId);
            Assert.Equal(subskillId, reqSub.SubskillId);

            // Assert: one domain event of correct type
            var events = ((AuditableEntity)reqSub).DomainEvents;
            Assert.Single(events);
            var createdEvt = Assert.IsType<JobRequiredSubskillCreated>(events.Single());
            Assert.Same(reqSub, createdEvt.JobRequiredSubskill);
        }

        [Fact]
        public void Update_WithDifferentIds_UpdatesProperties_AndQueuesUpdatedEvent()
        {
            // Arrange
            var originalJobId = Guid.NewGuid();
            var originalSubskillId = Guid.NewGuid();
            var reqSub = JobRequiredSubskill.Create(originalJobId, originalSubskillId);

            var beforeCount = ((AuditableEntity)reqSub).DomainEvents.Count;

            var newJobId = Guid.NewGuid();
            var newSubskillId = Guid.NewGuid();

            // Act
            var updated = reqSub.Update(newJobId, newSubskillId);

            // Assert: updated properties
            Assert.Equal(newJobId, updated.JobId);
            Assert.Equal(newSubskillId, updated.SubskillId);
            Assert.Same(reqSub, updated);

            // Assert: one new event appended
            var events = ((AuditableEntity)reqSub).DomainEvents;
            Assert.Equal(beforeCount + 1, events.Count);
            var updatedEvt = events.Last() as JobRequiredSubskillUpdated;
            Assert.NotNull(updatedEvt);
            Assert.Same(reqSub, updatedEvt!.JobRequiredSubskill);
        }

        [Fact]
        public void Update_WithSameIds_DoesNotChangeProperties_ButStillQueuesUpdatedEvent()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var subskillId = Guid.NewGuid();
            var reqSub = JobRequiredSubskill.Create(jobId, subskillId);

            var beforeCount = ((AuditableEntity)reqSub).DomainEvents.Count;

            // Act
            reqSub.Update(jobId, subskillId);

            // Assert: properties unchanged
            Assert.Equal(jobId, reqSub.JobId);
            Assert.Equal(subskillId, reqSub.SubskillId);

            // Assert: one new event appended
            var events = ((AuditableEntity)reqSub).DomainEvents;
            Assert.Equal(beforeCount + 1, events.Count);
            Assert.IsType<JobRequiredSubskillUpdated>(events.Last());
        }
    }
}
