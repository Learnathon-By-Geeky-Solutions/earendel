using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Events;
using TalentMesh.Framework.Core.Domain;

namespace TalentMesh.Module.Job.Tests
{
    public class JobsTests
    {
        [Fact]
        public void Create_WithValidInfo_SetsAllPropertiesAndQueuesJobCreatedEvent()
        {
            // Arrange
            var info = new JobInfo
            {
                Name = "Backend Developer",
                Description = "Work on APIs",
                Requirments = "C#, .NET",
                Location = "Dhaka",
                JobType = "Full-Time",
                ExperienceLevel = "Mid",
                Salary = "60000",
                PostedById = Guid.NewGuid()
            };

            // Act
            var job = Jobs.Create(info);

            // Assert properties
            Assert.Equal(info.Name, job.Name);
            Assert.Equal(info.Description, job.Description);
            Assert.Equal(info.Requirments, job.Requirments);
            Assert.Equal(info.Location, job.Location);
            Assert.Equal(info.JobType, job.JobType);
            Assert.Equal(info.ExperienceLevel, job.ExperienceLevel);
            Assert.Equal(info.Salary, job.Salary);
            Assert.Equal(info.PostedById, job.PostedById);
            Assert.Equal("Pending", job.PaymentStatus);

            // Assert exactly one JobCreated event was queued
            var events = ((AuditableEntity)job).DomainEvents;
            Assert.Single(events);
            var createdEvt = Assert.IsType<JobCreated>(events.Single());
            Assert.Same(job, createdEvt.User);
        }

        [Fact]
        public void Update_WithDifferentValues_AppendsSingleJobUpdatedEvent()
        {
            // Arrange: create initial job (queues JobCreated)
            var info = new JobInfo
            {
                Name = "Backend Developer",
                Description = "Work on APIs",
                Requirments = "C#, .NET",
                Location = "Dhaka",
                JobType = "Full-Time",
                ExperienceLevel = "Mid",
                Salary = "60000",
                PostedById = Guid.NewGuid()
            };
            var job = Jobs.Create(info);

            // record how many events so far (should be 1)
            var beforeCount = ((AuditableEntity)job).DomainEvents.Count;

            // Act: update everything
            job.Update(
                name: "Frontend Developer",
                description: "Build UIs",
                requirments: "Vue.js, HTML",
                location: "Chittagong",
                jobType: "Contract",
                experienceLevel: "Senior",
                salary: "80000",
                paymentStatus: "Paid"
            );

            // Assert: properties changed
            Assert.Equal("Frontend Developer", job.Name);
            Assert.Equal("Build UIs", job.Description);
            Assert.Equal("Vue.js, HTML", job.Requirments);
            Assert.Equal("Chittagong", job.Location);
            Assert.Equal("Contract", job.JobType);
            Assert.Equal("Senior", job.ExperienceLevel);
            Assert.Equal("80000", job.Salary);
            Assert.Equal("Paid", job.PaymentStatus);

            // Assert exactly one new event appended
            var events = ((AuditableEntity)job).DomainEvents;
            Assert.Equal(beforeCount + 1, events.Count);
            var updatedEvt = events.Last();
            Assert.IsType<JobUpdated>(updatedEvt);
            Assert.Same(job, ((JobUpdated)updatedEvt).User);
        }

        [Fact]
        public void Create_WithNullOptionalFields_SetsDefaults()
        {
            // Arrange: Description and Salary are optional
            var info = new JobInfo
            {
                Name = "Tester",
                Description = null,
                Requirments = "Attention to detail",
                Location = "Remote",
                JobType = "Part-Time",
                ExperienceLevel = "Junior",
                Salary = null,               // null salary
                PostedById = Guid.NewGuid()
            };

            // Act
            var job = Jobs.Create(info);

            // Assert
            Assert.Equal(info.Name, job.Name);
            Assert.Null(job.Description);
            Assert.Equal(string.Empty, job.Salary);    // null → empty string
            Assert.Equal("Pending", job.PaymentStatus);

            // Event
            var evt = ((AuditableEntity)job).DomainEvents.Single();
            Assert.IsType<JobCreated>(evt);
        }

        [Fact]
        public void Update_WithSameValues_DoesNotChangeProperties_ButStillQueuesEvent()
        {
            // Arrange
            var info = new JobInfo
            {
                Name = "DevOps",
                Description = "CI/CD",
                Requirments = "Jenkins",
                Location = "Onsite",
                JobType = "Full-Time",
                ExperienceLevel = "Senior",
                Salary = "100000",
                PostedById = Guid.NewGuid()
            };
            var job = Jobs.Create(info);
            var beforeEvents = ((AuditableEntity)job).DomainEvents.Count;

            // capture original values
            var origName = job.Name;
            var origDesc = job.Description;
            var origReqs = job.Requirments;
            var origLoc = job.Location;
            var origType = job.JobType;
            var origExp = job.ExperienceLevel;
            var origSal = job.Salary;
            var origPayStat = job.PaymentStatus;

            // Act: update using same values (different casing)
            job.Update(
                name: "DEVOPS",                 // same ignoring case
                description: "ci/cd",
                requirments: "jenkins",
                location: "onsite",
                jobType: "FULL-TIME",
                experienceLevel: "SENIOR",
                salary: "100000",
                paymentStatus: "PENDING"
            );

            // Assert: properties unchanged
            Assert.Equal(origName, job.Name);
            Assert.Equal(origDesc, job.Description);
            Assert.Equal(origReqs, job.Requirments);
            Assert.Equal(origLoc, job.Location);
            Assert.Equal(origType, job.JobType);
            Assert.Equal(origExp, job.ExperienceLevel);
            Assert.Equal(origSal, job.Salary);
            Assert.Equal(origPayStat, job.PaymentStatus);

            // but a new JobUpdated event is still queued
            var afterEvents = ((AuditableEntity)job).DomainEvents.Count;
            Assert.Equal(beforeEvents + 1, afterEvents);
            Assert.IsType<JobUpdated>(((AuditableEntity)job).DomainEvents.Last());
        }

        [Fact]
        public void Update_PartialFields_OnlyUpdatesThose()
        {
            // Arrange
            var info = new JobInfo
            {
                Name = "Engineer",
                Description = "Design",
                Requirments = "CAD",
                Location = "Field",
                JobType = "Contract",
                ExperienceLevel = "Mid",
                Salary = "70000",
                PostedById = Guid.NewGuid()
            };
            var job = Jobs.Create(info);
            var beforeEvents = ((AuditableEntity)job).DomainEvents.Count;

            // Act: only update Location and Salary
            job.Update(
                name: null,
                description: null,
                requirments: null,
                location: "Headquarters",
                jobType: null,
                experienceLevel: null,
                salary: "75000",
                paymentStatus: null
            );

            // Assert: only those two changed
            Assert.Equal("Headquarters", job.Location);
            Assert.Equal("75000", job.Salary);

            // others remain as originally set
            Assert.Equal(info.Name, job.Name);
            Assert.Equal(info.Description, job.Description);
            Assert.Equal(info.Requirments, job.Requirments);
            Assert.Equal(info.JobType, job.JobType);
            Assert.Equal(info.ExperienceLevel, job.ExperienceLevel);
            Assert.Equal("Pending", job.PaymentStatus);

            // and exactly one more event queued
            Assert.Equal(beforeEvents + 1, ((AuditableEntity)job).DomainEvents.Count);
            Assert.IsType<JobUpdated>(((AuditableEntity)job).DomainEvents.Last());
        }
    }
}
