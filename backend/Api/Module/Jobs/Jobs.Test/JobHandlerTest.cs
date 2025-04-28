using Moq;
using Xunit;
using TalentMesh.Module.Job.Application.Jobs.Create.v1;
using TalentMesh.Module.Job.Application.Jobs.Delete.v1;
using TalentMesh.Module.Job.Application.Jobs.Get.v1;
using TalentMesh.Module.Job.Application.Jobs.Search.v1;
using TalentMesh.Module.Job.Application.Jobs.Update.v1;
using TalentMesh.Module.Job.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Job.Tests
{
    public class JobHandlerTests
    {
        private readonly Mock<IRepository<Jobs>> _repositoryMock;
        private readonly Mock<IReadRepository<Jobs>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateJobHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteJobHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetJobHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchJobsHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateJobHandler>> _updateLoggerMock;

        private readonly CreateJobHandler _createHandler;
        private readonly DeleteJobHandler _deleteHandler;
        private readonly GetJobHandler _getHandler;
        private readonly SearchJobsHandler _searchHandler;
        private readonly UpdateJobHandler _updateHandler;

        public JobHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<Jobs>>();
            _readRepositoryMock = new Mock<IReadRepository<Jobs>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateJobHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteJobHandler>>();
            _getLoggerMock = new Mock<ILogger<GetJobHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchJobsHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateJobHandler>>();

            _createHandler = new CreateJobHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteJobHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetJobHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchJobsHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateJobHandler(_updateLoggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task CreateJob_ReturnsJobResponse()
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

            // Using constructor parameters
            var request = new CreateJobCommand(
                jobInfo.Name,
                jobInfo.Description,
                jobInfo.Requirments,
                jobInfo.Location,
                jobInfo.JobType,
                jobInfo.ExperienceLevel,
                jobInfo.Salary,
                jobInfo.PostedById
            );

            var expectedJob = Jobs.Create(jobInfo);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Jobs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedJob);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Jobs>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteJob_DeletesSuccessfully()
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
            var existingJob = Jobs.Create(jobInfo);
            var jobId = existingJob.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingJob);

            // Act
            await _deleteHandler.Handle(new DeleteJobCommand(jobId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingJob, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteJob_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var jobId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Jobs?)null);

            // Act & Assert
            await Assert.ThrowsAsync<JobNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteJobCommand(jobId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetJob_ReturnsJobResponse()
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
            var expectedJob = Jobs.Create(jobInfo);
            var jobId = expectedJob.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(jobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedJob);

            _cacheServiceMock.Setup(cache => cache.GetAsync<JobResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobResponse?)null);

            // Act
            var result = await _getHandler.Handle(new GetJobRequest(jobId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedJob.Id, result.Id);
            Assert.Equal(expectedJob.Name, result.Name);
            Assert.Equal(expectedJob.Description, result.Description);
            Assert.Equal(expectedJob.Requirments, result.Requirments);
            Assert.Equal(expectedJob.Location, result.Location);
            Assert.Equal(expectedJob.JobType, result.JobType);
            Assert.Equal(expectedJob.ExperienceLevel, result.ExperienceLevel);
            Assert.Equal(expectedJob.Salary, result.Salary);
            Assert.Equal(expectedJob.PostedById, result.PostedById);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(jobId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<JobResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetJob_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var jobId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(jobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Jobs?)null);

            // Act & Assert
            await Assert.ThrowsAsync<JobNotFoundException>(() =>
                _getHandler.Handle(new GetJobRequest(jobId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(jobId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchJobs_ReturnsPagedJobResponse()
        {
            // Arrange
            var request = new SearchJobsCommand
            {
                Location = "Remote",
                JobType = "Full-time",
                ExperienceLevel = "Mid-Senior",
                PageNumber = 1,
                PageSize = 10
            };

            // Create domain objects
            var job1 = Jobs.Create(new JobInfo
            {
                Name = "Software Developer",
                Description = "Developing software applications",
                Requirments = "C#, .NET Core, Azure",
                Location = "Remote",
                JobType = "Full-time",
                ExperienceLevel = "Mid-Senior",
                Salary = "100,000 - 120,000",
                PostedById = Guid.NewGuid()
            });
            job1.Update(name: null, description: null, requirments: null, location: null, jobType: null, experienceLevel: null, salary: null, paymentStatus: "Pending");

            var job2 = Jobs.Create(new JobInfo
            {
                Name = "Frontend Developer",
                Description = "Developing frontend applications",
                Requirments = "React, TypeScript, CSS",
                Location = "Remote",
                JobType = "Full-time",
                ExperienceLevel = "Mid-Senior",
                Salary = "90,000 - 110,000",
                PostedById = Guid.NewGuid()
            });
            job2.Update(name: null, description: null, requirments: null, location: null, jobType: null, experienceLevel: null, salary: null, paymentStatus: "Paid");

            var jobs = new List<Jobs> { job1, job2 };
            var totalCount = jobs.Count;

            // Use a callback to return the list of Jobs
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchJobSpecs>(), It.IsAny<CancellationToken>()))
                .Returns((SearchJobSpecs specs, CancellationToken token) => Task.FromResult(jobs));

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchJobSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchJobSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchJobSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateJob_ReturnsUpdatedJobResponse()
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
            var existingJob = Jobs.Create(jobInfo);
            var jobId = existingJob.Id;

            // Using constructor parameters
            var request = new UpdateJobCommand(
                jobId,
                "Senior Software Developer",
                "Developing enterprise software applications",
                "C#, .NET Core, Azure, Microservices",
                "Hybrid",
                "Full-time",
                "Senior",
                "120,000 - 150,000"
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingJob);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(jobId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Jobs>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateJob_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var jobId = Guid.NewGuid();

            // Using constructor parameters
            var request = new UpdateJobCommand(
                jobId,
                "Senior Software Developer",
                "Developing enterprise software applications",
                "C#, .NET Core, Azure, Microservices",
                "Hybrid",
                "Full-time",
                "Senior",
                "120,000 - 150,000"
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Jobs?)null);

            // Act & Assert
            await Assert.ThrowsAsync<JobNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}