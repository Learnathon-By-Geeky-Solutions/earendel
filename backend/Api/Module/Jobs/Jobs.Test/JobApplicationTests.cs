using System;
using System.Linq;
using Xunit;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Events;
using TalentMesh.Framework.Core.Domain;

namespace TalentMesh.Module.Job.Tests
{

    public class JobApplicationTests
    {
        [Fact]
        public void Create_SetsPropertiesAndQueuesJobApplicationCreatedEvent()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var candidateId = Guid.NewGuid();
            var coverLetter = "Looking forward to joining.";

            // Act
            var application = JobApplication.Create(jobId, candidateId, coverLetter);

            // Assert
            Assert.Equal(jobId, application.JobId);
            Assert.Equal(candidateId, application.CandidateId);
            Assert.Equal("Applied", application.Status);
            Assert.Equal(coverLetter, application.CoverLetter);
            Assert.InRange(application.ApplicationDate,
                           DateTime.UtcNow.AddSeconds(-5),
                           DateTime.UtcNow);

            // exactly one JobApplicationCreated event
            var events = ((AuditableEntity)application).DomainEvents;
            Assert.Single(events);
            var createdEvt = Assert.IsType<JobApplicationCreated>(events.Single());
            Assert.Same(application, createdEvt.JobApplication);
        }

        [Fact]
        public void Update_WithNewStatusAndCoverLetter_AppendsSingleJobApplicationUpdatedEvent()
        {
            // Arrange
            var application = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "Initial cover");
            var beforeCount = ((AuditableEntity)application).DomainEvents.Count;

            // Act
            application.Update(
                status: "Under Review",
                coverLetter: "Updated cover"
            );

            // Assert
            Assert.Equal("Under Review", application.Status);
            Assert.Equal("Updated cover", application.CoverLetter);

            var events = ((AuditableEntity)application).DomainEvents;
            Assert.Equal(beforeCount + 1, events.Count);
            var updatedEvt = events.Last();
            Assert.IsType<JobApplicationUpdated>(updatedEvt);
            Assert.Same(application, ((JobApplicationUpdated)updatedEvt).JobApplication);
        }
    

        [Fact]
        public void Create_WithNullCoverLetter_SetsCoverLetterNull()
        {
            // Act
            var application = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid(), coverLetter: null);

            // Assert
            Assert.Null(application.CoverLetter);
            var evt = ((AuditableEntity)application).DomainEvents.Single();
            Assert.IsType<JobApplicationCreated>(evt);
        }

        [Fact]
        public void Update_OnlyStatus_ChangesStatusOnly()
        {
            // Arrange
            var app = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "Initial");
            var beforeCover = app.CoverLetter;
            var beforeEvents = ((AuditableEntity)app).DomainEvents.Count;

            // Act
            app.Update(status: "Accepted", coverLetter: null);

            // Assert
            Assert.Equal("Accepted", app.Status);
            Assert.Equal(beforeCover, app.CoverLetter);

            Assert.Equal(beforeEvents + 1, ((AuditableEntity)app).DomainEvents.Count);
            Assert.IsType<JobApplicationUpdated>(((AuditableEntity)app).DomainEvents.Last());
        }

        [Fact]
        public void Update_OnlyCoverLetter_ChangesCoverLetterOnly()
        {
            // Arrange
            var app = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "Old cover");
            var beforeStatus = app.Status;
            var beforeEvents = ((AuditableEntity)app).DomainEvents.Count;

            // Act
            app.Update(status: null, coverLetter: "New cover");

            // Assert
            Assert.Equal(beforeStatus, app.Status);
            Assert.Equal("New cover", app.CoverLetter);

            Assert.Equal(beforeEvents + 1, ((AuditableEntity)app).DomainEvents.Count);
            Assert.IsType<JobApplicationUpdated>(((AuditableEntity)app).DomainEvents.Last());
        }
    }
}
