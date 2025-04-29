using Xunit;
using TalentMesh.Module.Job.Domain;
using System;

namespace TalentMesh.Module.Job.Tests
{
    public class JobDomainTests
    {
        [Fact]
        public void Jobs_Create_SetsPropertiesCorrectly()
        {
            // Arrange
            var jobInfo = new JobInfo
            {
                Name = "Software Developer",
                Description = "Developing software applications",
                Requirments = "C#, .NET Core, Azure",
                Location = "Remote",
                JobType = "Full-time",
                ExperienceLevel = "Mid-Senior",
                Salary = "100,000 - 120,000",
                PostedById = Guid.NewGuid()
            };

            // Act
            var job = Jobs.Create(jobInfo);

            // Assert
            Assert.Equal(jobInfo.Name, job.Name);
            Assert.Equal(jobInfo.Description, job.Description);
            Assert.Equal(jobInfo.Requirments, job.Requirments);
            Assert.Equal(jobInfo.Location, job.Location);
            Assert.Equal(jobInfo.JobType, job.JobType);
            Assert.Equal(jobInfo.ExperienceLevel, job.ExperienceLevel);
            Assert.Equal(jobInfo.Salary, job.Salary);
            Assert.Equal(jobInfo.PostedById, job.PostedById);
            Assert.Equal("Pending", job.PaymentStatus);
        }

        [Fact]
        public void Jobs_Update_WithNullValues_DoesNotChangeProperties()
        {
            // Arrange
            var jobInfo = new JobInfo
            {
                Name = "Software Developer",
                Description = "Developing software applications",
                Requirments = "C#, .NET Core, Azure",
                Location = "Remote",
                JobType = "Full-time",
                ExperienceLevel = "Mid-Senior",
                Salary = "100,000 - 120,000",
                PostedById = Guid.NewGuid()
            };
            var job = Jobs.Create(jobInfo);

            // Act
            var jobUpdateDetails = new JobUpdateDetails
            {
                Name = null,
                Description = null,
                Requirments = null,
                Location = null,
                JobType = null,
                ExperienceLevel = null,
                Salary = null,
                PaymentStatus = "Pending"
            };

            // Update the job using the new structure
            job.Update(jobUpdateDetails);

            // Assert
            Assert.Equal(jobInfo.Name, job.Name);
            Assert.Equal(jobInfo.Description, job.Description);
            Assert.Equal(jobInfo.Requirments, job.Requirments);
            Assert.Equal(jobInfo.Location, job.Location);
            Assert.Equal(jobInfo.JobType, job.JobType);
            Assert.Equal(jobInfo.ExperienceLevel, job.ExperienceLevel);
            Assert.Equal(jobInfo.Salary, job.Salary);
            Assert.Equal("Pending", job.PaymentStatus);
        }

        [Fact]
        public void Jobs_Update_WithValidValues_ChangesProperties()
        {
            // Arrange
            var jobInfo = new JobInfo
            {
                Name = "Software Developer",
                Description = "Developing software applications",
                Requirments = "C#, .NET Core, Azure",
                Location = "Remote",
                JobType = "Full-time",
                ExperienceLevel = "Mid-Senior",
                Salary = "100,000 - 120,000",
                PostedById = Guid.NewGuid()
            };
            var job = Jobs.Create(jobInfo);

            // New values
            var newName = "Senior Software Developer";
            var newDescription = "Leading development team";
            var newRequirements = "C#, .NET Core, Azure, Leadership";
            var newLocation = "Hybrid";
            var newJobType = "Contract";
            var newExperienceLevel = "Senior";
            var newSalary = "120,000 - 150,000";
            var newPaymentStatus = "Paid";

            var jobUpdateDetails = new JobUpdateDetails
            {
                Name = newName,
                Description = newDescription,
                Requirments = newRequirements,
                Location = newLocation,
                JobType = newJobType,
                ExperienceLevel = newExperienceLevel,
                Salary = newSalary,
                PaymentStatus = newPaymentStatus
            };

            // Act
            job.Update(jobUpdateDetails);
            // Assert
            Assert.Equal(newName, job.Name);
            Assert.Equal(newDescription, job.Description);
            Assert.Equal(newRequirements, job.Requirments);
            Assert.Equal(newLocation, job.Location);
            Assert.Equal(newJobType, job.JobType);
            Assert.Equal(newExperienceLevel, job.ExperienceLevel);
            Assert.Equal(newSalary, job.Salary);
            Assert.Equal(newPaymentStatus, job.PaymentStatus);
        }

        [Fact]
        public void JobApplication_Create_SetsPropertiesCorrectly()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var candidateId = Guid.NewGuid();
            var coverLetter = "I am excited to apply for this position...";

            // Act
            var jobApplication = JobApplication.Create(jobId, candidateId, coverLetter);

            // Assert
            Assert.Equal(jobId, jobApplication.JobId);
            Assert.Equal(candidateId, jobApplication.CandidateId);
            Assert.Equal(coverLetter, jobApplication.CoverLetter);
            Assert.Equal("Applied", jobApplication.Status);
        }

        [Fact]
        public void JobApplication_Update_WithNullValues_DoesNotChangeProperties()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var candidateId = Guid.NewGuid();
            var coverLetter = "I am excited to apply for this position...";
            var jobApplication = JobApplication.Create(jobId, candidateId, coverLetter);

            // Act
            jobApplication.Update(null, null);

            // Assert
            Assert.Equal("Applied", jobApplication.Status);
            Assert.Equal(coverLetter, jobApplication.CoverLetter);
        }

        [Fact]
        public void JobApplication_Update_WithValidValues_ChangesProperties()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var candidateId = Guid.NewGuid();
            var coverLetter = "I am excited to apply for this position...";
            var jobApplication = JobApplication.Create(jobId, candidateId, coverLetter);

            // New values
            var newStatus = "Under Review";
            var newCoverLetter = "Updated cover letter content...";

            // Act
            jobApplication.Update(newStatus, newCoverLetter);

            // Assert
            Assert.Equal(newStatus, jobApplication.Status);
            Assert.Equal(newCoverLetter, jobApplication.CoverLetter);
        }

        [Fact]
        public void JobRequiredSkill_Create_SetsPropertiesCorrectly()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var skillId = Guid.NewGuid();

            // Act
            var jobRequiredSkill = JobRequiredSkill.Create(jobId, skillId);

            // Assert
            Assert.Equal(jobId, jobRequiredSkill.JobId);
            Assert.Equal(skillId, jobRequiredSkill.SkillId);
        }

        [Fact]
        public void JobRequiredSkill_Update_WithSameValues_DoesNotChangeProperties()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var skillId = Guid.NewGuid();
            var jobRequiredSkill = JobRequiredSkill.Create(jobId, skillId);

            // Act
            jobRequiredSkill.Update(jobId, skillId);

            // Assert
            Assert.Equal(jobId, jobRequiredSkill.JobId);
            Assert.Equal(skillId, jobRequiredSkill.SkillId);
        }

        [Fact]
        public void JobRequiredSkill_Update_WithDifferentValues_ChangesProperties()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var skillId = Guid.NewGuid();
            var jobRequiredSkill = JobRequiredSkill.Create(jobId, skillId);

            // New values
            var newJobId = Guid.NewGuid();
            var newSkillId = Guid.NewGuid();

            // Act
            jobRequiredSkill.Update(newJobId, newSkillId);

            // Assert
            Assert.Equal(newJobId, jobRequiredSkill.JobId);
            Assert.Equal(newSkillId, jobRequiredSkill.SkillId);
        }

        [Fact]
        public void JobRequiredSubskill_Create_SetsPropertiesCorrectly()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var subskillId = Guid.NewGuid();

            // Act
            var jobRequiredSubskill = JobRequiredSubskill.Create(jobId, subskillId);

            // Assert
            Assert.Equal(jobId, jobRequiredSubskill.JobId);
            Assert.Equal(subskillId, jobRequiredSubskill.SubskillId);
        }

        [Fact]
        public void JobRequiredSubskill_Update_WithSameValues_DoesNotChangeProperties()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var subskillId = Guid.NewGuid();
            var jobRequiredSubskill = JobRequiredSubskill.Create(jobId, subskillId);

            // Act
            jobRequiredSubskill.Update(jobId, subskillId);

            // Assert
            Assert.Equal(jobId, jobRequiredSubskill.JobId);
            Assert.Equal(subskillId, jobRequiredSubskill.SubskillId);
        }

        [Fact]
        public void JobRequiredSubskill_Update_WithDifferentValues_ChangesProperties()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var subskillId = Guid.NewGuid();
            var jobRequiredSubskill = JobRequiredSubskill.Create(jobId, subskillId);

            // New values
            var newJobId = Guid.NewGuid();
            var newSubskillId = Guid.NewGuid();

            // Act
            jobRequiredSubskill.Update(newJobId, newSubskillId);

            // Assert
            Assert.Equal(newJobId, jobRequiredSubskill.JobId);
            Assert.Equal(newSubskillId, jobRequiredSubskill.SubskillId);
        }
    }
}