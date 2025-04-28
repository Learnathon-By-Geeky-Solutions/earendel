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
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Create.v1;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Delete.v1;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Get.v1;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Search.v1;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Update.v1;

namespace TalentMesh.Module.Job.Tests
{
    public class JobRequiredSubskillHandlerTests
    {
        private readonly Mock<IRepository<JobRequiredSubskill>> _repositoryMock;
        private readonly Mock<IReadRepository<JobRequiredSubskill>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateJobRequiredSubskillHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteJobRequiredSubskillHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetJobRequiredSubskillHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchJobRequiredSubskillHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateJobRequiredSubskillHandler>> _updateLoggerMock;

        private readonly CreateJobRequiredSubskillHandler _createHandler;
        private readonly DeleteJobRequiredSubskillHandler _deleteHandler;
        private readonly GetJobRequiredSubskillHandler _getHandler;
        private readonly SearchJobRequiredSubskillHandler _searchHandler;
        private readonly UpdateJobRequiredSubskillHandler _updateHandler;

        public JobRequiredSubskillHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<JobRequiredSubskill>>();
            _readRepositoryMock = new Mock<IReadRepository<JobRequiredSubskill>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateJobRequiredSubskillHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteJobRequiredSubskillHandler>>();
            _getLoggerMock = new Mock<ILogger<GetJobRequiredSubskillHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchJobRequiredSubskillHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateJobRequiredSubskillHandler>>();

            _createHandler = new CreateJobRequiredSubskillHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteJobRequiredSubskillHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetJobRequiredSubskillHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchJobRequiredSubskillHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateJobRequiredSubskillHandler(_updateLoggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task CreateJobRequiredSubskill_ReturnsJobRequiredSubskillResponse()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var subskillId = Guid.NewGuid();
            var request = new CreateJobRequiredSubskillCommand(jobId, subskillId);
            var expectedJobRequiredSubskill = JobRequiredSubskill.Create(request.JobId, request.SubskillId);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<JobRequiredSubskill>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedJobRequiredSubskill);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<JobRequiredSubskill>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteJobRequiredSubskill_DeletesSuccessfully()
        {
            // Arrange
            var existingJobRequiredSubskill = JobRequiredSubskill.Create(Guid.NewGuid(), Guid.NewGuid());
            var jobRequiredSubskillId = existingJobRequiredSubskill.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobRequiredSubskillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingJobRequiredSubskill);

            // Act
            await _deleteHandler.Handle(new DeleteJobRequiredSubskillCommand(jobRequiredSubskillId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingJobRequiredSubskill, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobRequiredSubskillId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteJobRequiredSubskill_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var jobRequiredSubskillId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobRequiredSubskillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobRequiredSubskill?)null);

            // Act & Assert
            await Assert.ThrowsAsync<JobRequiredSubskillNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteJobRequiredSubskillCommand(jobRequiredSubskillId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobRequiredSubskillId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetJobRequiredSubskill_ReturnsJobRequiredSubskillResponse()
        {
            // Arrange
            var expectedJobRequiredSubskill = JobRequiredSubskill.Create(Guid.NewGuid(), Guid.NewGuid());
            var jobRequiredSubskillId = expectedJobRequiredSubskill.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(jobRequiredSubskillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedJobRequiredSubskill);

            _cacheServiceMock.Setup(cache => cache.GetAsync<JobRequiredSubskillResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobRequiredSubskillResponse?)null);

            // Act
            var result = await _getHandler.Handle(new GetJobRequiredSubskillRequest(jobRequiredSubskillId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedJobRequiredSubskill.Id, result.Id);
            Assert.Equal(expectedJobRequiredSubskill.JobId, result.JobId);
            Assert.Equal(expectedJobRequiredSubskill.SubskillId, result.SubskillId);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(jobRequiredSubskillId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<JobRequiredSubskillResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetJobRequiredSubskill_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var jobRequiredSubskillId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(jobRequiredSubskillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobRequiredSubskill?)null);

            // Act & Assert
            await Assert.ThrowsAsync<JobRequiredSubskillNotFoundException>(() =>
                _getHandler.Handle(new GetJobRequiredSubskillRequest(jobRequiredSubskillId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(jobRequiredSubskillId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchJobRequiredSubskills_ReturnsPagedJobRequiredSubskillResponse()
        {
            // Arrange
            var request = new SearchJobRequiredSubskillCommand
            {
                JobId = Guid.NewGuid(),
                PageNumber = 1,
                PageSize = 10
            };

            var jobRequiredSubskills = new List<JobRequiredSubskillResponse>
            {
                new JobRequiredSubskillResponse(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                ),
                new JobRequiredSubskillResponse(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                )
            };
            var totalCount = jobRequiredSubskills.Count;

            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchJobRequiredSubskillSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(jobRequiredSubskills);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchJobRequiredSubskillSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchJobRequiredSubskillSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchJobRequiredSubskillSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateJobRequiredSubskill_ReturnsUpdatedJobRequiredSubskillResponse()
        {
            // Arrange
            var existingJobRequiredSubskill = JobRequiredSubskill.Create(Guid.NewGuid(), Guid.NewGuid());
            var jobRequiredSubskillId = existingJobRequiredSubskill.Id;
            var request = new UpdateJobRequiredSubskillCommand(
                jobRequiredSubskillId,
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobRequiredSubskillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingJobRequiredSubskill);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(jobRequiredSubskillId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobRequiredSubskillId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<JobRequiredSubskill>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateJobRequiredSubskill_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var jobRequiredSubskillId = Guid.NewGuid();
            var request = new UpdateJobRequiredSubskillCommand(jobRequiredSubskillId, Guid.NewGuid(), Guid.NewGuid());

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobRequiredSubskillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobRequiredSubskill?)null);

            // Act & Assert
            await Assert.ThrowsAsync<JobRequiredSubskillNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobRequiredSubskillId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}