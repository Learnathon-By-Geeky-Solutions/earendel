using Moq;
using Xunit;
using TalentMesh.Module.Job.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Job.Application.JobApplication.Create.v1;
using TalentMesh.Module.Job.Application.JobApplication.Delete.v1;
using TalentMesh.Module.Job.Application.JobApplication.Get.v1;
using TalentMesh.Module.Job.Application.JobApplication.Search.v1;
using TalentMesh.Module.Job.Application.JobApplication.Update.v1;
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Module.Job.Application.Jobs.Create.v1;

namespace TalentMesh.Module.Job.Tests
{
    public class JobApplicationHandlerTests
    {
        private readonly Mock<IRepository<JobApplication>> _repositoryMock;
        private readonly Mock<IReadRepository<JobApplication>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IMessageBus> _messageServiceMock;
        private readonly Mock<ILogger<CreateJobApplicationHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteJobApplicationHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetJobApplicationHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchJobApplicationHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateJobApplicationHandler>> _updateLoggerMock;

        private readonly CreateJobApplicationHandler _createHandler;
        private readonly DeleteJobApplicationHandler _deleteHandler;
        private readonly GetJobApplicationHandler _getHandler;
        private readonly SearchJobApplicationHandler _searchHandler;
        private readonly UpdateJobApplicationHandler _updateHandler;

        public JobApplicationHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<JobApplication>>();
            _readRepositoryMock = new Mock<IReadRepository<JobApplication>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _messageServiceMock = new Mock<IMessageBus>();
            _createLoggerMock = new Mock<ILogger<CreateJobApplicationHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteJobApplicationHandler>>();
            _getLoggerMock = new Mock<ILogger<GetJobApplicationHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchJobApplicationHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateJobApplicationHandler>>();

            _createHandler = new CreateJobApplicationHandler(_createLoggerMock.Object, _repositoryMock.Object, _messageServiceMock.Object);
            _deleteHandler = new DeleteJobApplicationHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetJobApplicationHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchJobApplicationHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateJobApplicationHandler(_updateLoggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task CreateJobApplication_ReturnsJobApplicationResponse()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var candidateId = Guid.NewGuid();
            var coverLetter = "I am excited to apply for this position...";
            var request = new CreateJobApplicationCommand(jobId, candidateId, coverLetter);
            var expectedJobApplication = JobApplication.Create(request.JobId, request.CandidateId, request.CoverLetter);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<JobApplication>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedJobApplication);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<JobApplication>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteJobApplication_DeletesSuccessfully()
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
                Salary = "100000",
                PostedById = Guid.NewGuid()
            };

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
            var jobId = expectedJob.Id;

            var existingJobApplication = JobApplication.Create(jobId, Guid.NewGuid(), "Cover letter content");
            var jobApplicationId = existingJobApplication.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobApplicationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingJobApplication);

            // Act
            await _deleteHandler.Handle(new DeleteJobApplicationCommand(jobApplicationId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingJobApplication, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteJobApplication_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var jobApplicationId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobApplicationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobApplication?)null);

            // Act & Assert
            await Assert.ThrowsAsync<JobApplicationNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteJobApplicationCommand(jobApplicationId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobApplicationId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetJobApplication_ReturnsJobApplicationResponse()
        {
            // Arrange
            var expectedJobApplication = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "Cover letter content");
            var jobApplicationId = expectedJobApplication.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(jobApplicationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedJobApplication);

            _cacheServiceMock.Setup(cache => cache.GetAsync<JobApplicationResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobApplicationResponse?)null);

            // Act
            var result = await _getHandler.Handle(new GetJobApplicationRequest(jobApplicationId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedJobApplication.Id, result.Id);
            Assert.Equal(expectedJobApplication.JobId, result.JobId);
            Assert.Equal(expectedJobApplication.CandidateId, result.CandidateId);
            Assert.Equal(expectedJobApplication.ApplicationDate, result.ApplicationDate);
            Assert.Equal(expectedJobApplication.Status, result.Status);
            Assert.Equal(expectedJobApplication.CoverLetter, result.CoverLetter);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(jobApplicationId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<JobApplicationResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetJobApplication_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var jobApplicationId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(jobApplicationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobApplication?)null);

            // Act & Assert
            await Assert.ThrowsAsync<JobApplicationNotFoundException>(() =>
                _getHandler.Handle(new GetJobApplicationRequest(jobApplicationId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(jobApplicationId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchJobApplications_ReturnsPagedJobApplicationResponse()
        {
            // Arrange
            var request = new SearchJobApplicationsCommand
            {
                JobId = Guid.NewGuid(),
                CandidateId = Guid.NewGuid(),
                Status = "Applied",
                PageNumber = 1,
                PageSize = 10
            };

            var jobApplications = new List<JobApplicationResponse>
            {
                new JobApplicationResponse(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    "Applied",
                    "Cover letter 1"
                ),
                new JobApplicationResponse(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    "Under Review",
                    "Cover letter 2"
                )
            };
            var totalCount = jobApplications.Count;

            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchJobApplicationSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(jobApplications);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchJobApplicationSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.Status == "Applied" &&
                item.CoverLetter == "Cover letter 1"
            );

            Assert.Contains(result.Items, item =>
                item.Status == "Under Review" &&
                item.CoverLetter == "Cover letter 2"
            );

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchJobApplicationSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchJobApplicationSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateJobApplication_ReturnsUpdatedJobApplicationResponse()
        {
            // Arrange
            var existingJobApplication = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "Original cover letter");
            var jobApplicationId = existingJobApplication.Id;
            var request = new UpdateJobApplicationCommand(
                existingJobApplication.Id,
                existingJobApplication.JobId,
                existingJobApplication.CandidateId,
                "Under Review",
                "Updated cover letter"
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobApplicationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingJobApplication);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(jobApplicationId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobApplicationId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<JobApplication>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateJobApplication_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var jobApplicationId = Guid.NewGuid();
            var jobApplicationJobID = Guid.NewGuid();
            var jobApplicationCandidate = Guid.NewGuid();
            var request = new UpdateJobApplicationCommand(jobApplicationId,jobApplicationJobID, jobApplicationCandidate, "Under Review", "Updated cover letter");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobApplicationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobApplication?)null);

            // Act & Assert
            await Assert.ThrowsAsync<JobApplicationNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobApplicationId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}