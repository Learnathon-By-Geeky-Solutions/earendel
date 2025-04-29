using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Infrastructure.Persistence;
using TalentMesh.Module.CandidateLogic.JobApplicationView;
using TalentMesh.Module.Job.Domain.Events;
using Microsoft.AspNetCore.Mvc;

namespace TalentMesh.Module.Job.Tests
{
    public class JobApplicationDomainTests
    {
        // Domain Entity Tests

        [Fact]
        public void JobApplication_Create_ShouldCreateWithCorrectData()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var candidateId = Guid.NewGuid();
            var coverLetter = "I'm interested in this position because...";

            // Act
            var jobApplication = JobApplication.Create(jobId, candidateId, coverLetter);

            // Assert
            Assert.Equal(jobId, jobApplication.JobId);
            Assert.Equal(candidateId, jobApplication.CandidateId);
            Assert.Equal("Applied", jobApplication.Status);
            Assert.Equal(coverLetter, jobApplication.CoverLetter);
            Assert.True(DateTime.UtcNow.Subtract(jobApplication.ApplicationDate).TotalMinutes < 1); // Created within the last minute
        }

        [Fact]
        public void JobApplication_Update_ShouldUpdateStatus()
        {
            // Arrange
            var jobApplication = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "Initial cover letter");
            var newStatus = "Under Review";

            // Act
            jobApplication.Update(newStatus, null);

            // Assert
            Assert.Equal(newStatus, jobApplication.Status);
        }

        [Fact]
        public void JobApplication_Update_ShouldUpdateCoverLetter()
        {
            // Arrange
            var jobApplication = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "Initial cover letter");
            var newCoverLetter = "Updated cover letter with more details";

            // Act
            jobApplication.Update(null, newCoverLetter);

            // Assert
            Assert.Equal(newCoverLetter, jobApplication.CoverLetter);
        }

        [Fact]
        public void JobRequiredSkill_Create_ShouldCreateWithCorrectData()
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
        public void JobRequiredSubskill_Create_ShouldCreateWithCorrectData()
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
        public void Jobs_Create_ShouldCreateWithCorrectData()
        {
            // Arrange
            var jobInfo = new JobInfo
            {
                Name = "Senior Software Engineer",
                Description = "We are looking for an experienced software engineer...",
                Requirments = "5+ years of experience in C#, .NET",
                Location = "Remote",
                JobType = "Full-time",
                ExperienceLevel = "Senior",
                Salary = "100,000 - 120,000 USD",
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
    }

    public class JobApplicationViewServiceTests
    {
        private readonly Mock<JobDbContext> _mockDbContext;
        private readonly Mock<DbSet<JobApplication>> _mockJobApplicationDbSet;

        public JobApplicationViewServiceTests()
        {
            _mockDbContext = new Mock<JobDbContext>();
            _mockJobApplicationDbSet = new Mock<DbSet<JobApplication>>();

            _mockDbContext.Setup(c => c.JobApplications).Returns(_mockJobApplicationDbSet.Object);
        }

        
    }
    
       
    
}

