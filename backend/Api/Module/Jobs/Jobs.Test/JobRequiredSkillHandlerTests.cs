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
using TalentMesh.Module.Job.Application.JobRequiredSkill.Create.v1;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Delete.v1;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Get.v1;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Search.v1;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Update.v1;

namespace TalentMesh.Module.Job.Tests
{
    public class JobRequiredSkillHandlerTests
    {
        private readonly Mock<IRepository<JobRequiredSkill>> _repositoryMock;
        private readonly Mock<IReadRepository<JobRequiredSkill>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateJobRequiredSkillHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteJobRequiredSkillHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetJobRequiredSkillHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchJobRequiredSkillHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateJobRequiredSkillHandler>> _updateLoggerMock;

        private readonly CreateJobRequiredSkillHandler _createHandler;
        private readonly DeleteJobRequiredSkillHandler _deleteHandler;
        private readonly GetJobRequiredSkillHandler _getHandler;
        private readonly SearchJobRequiredSkillHandler _searchHandler;
        private readonly UpdateJobRequiredSkillHandler _updateHandler;

        public JobRequiredSkillHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<JobRequiredSkill>>();
            _readRepositoryMock = new Mock<IReadRepository<JobRequiredSkill>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateJobRequiredSkillHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteJobRequiredSkillHandler>>();
            _getLoggerMock = new Mock<ILogger<GetJobRequiredSkillHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchJobRequiredSkillHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateJobRequiredSkillHandler>>();

            _createHandler = new CreateJobRequiredSkillHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteJobRequiredSkillHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetJobRequiredSkillHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchJobRequiredSkillHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateJobRequiredSkillHandler(_updateLoggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task CreateJobRequiredSkill_ReturnsJobRequiredSkillResponse()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var skillId = Guid.NewGuid();
            var request = new CreateJobRequiredSkillCommand(jobId, skillId);
            var expectedJobRequiredSkill = JobRequiredSkill.Create(request.JobId, request.SkillId);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<JobRequiredSkill>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedJobRequiredSkill);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<JobRequiredSkill>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteJobRequiredSkill_DeletesSuccessfully()
        {
            // Arrange
            var existingJobRequiredSkill = JobRequiredSkill.Create(Guid.NewGuid(), Guid.NewGuid());
            var jobRequiredSkillId = existingJobRequiredSkill.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobRequiredSkillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingJobRequiredSkill);

            // Act
            await _deleteHandler.Handle(new DeleteJobRequiredSkillCommand(jobRequiredSkillId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingJobRequiredSkill, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobRequiredSkillId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteJobRequiredSkill_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var jobRequiredSkillId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobRequiredSkillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobRequiredSkill?)null);

            // Act & Assert
            await Assert.ThrowsAsync<JobRequiredSkillNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteJobRequiredSkillCommand(jobRequiredSkillId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobRequiredSkillId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetJobRequiredSkill_ReturnsJobRequiredSkillResponse()
        {
            // Arrange
            var expectedJobRequiredSkill = JobRequiredSkill.Create(Guid.NewGuid(), Guid.NewGuid());
            var jobRequiredSkillId = expectedJobRequiredSkill.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(jobRequiredSkillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedJobRequiredSkill);

            _cacheServiceMock.Setup(cache => cache.GetAsync<JobRequiredSkillResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobRequiredSkillResponse?)null);

            // Act
            var result = await _getHandler.Handle(new GetJobRequiredSkillRequest(jobRequiredSkillId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedJobRequiredSkill.Id, result.Id);
            Assert.Equal(expectedJobRequiredSkill.JobId, result.JobId);
            Assert.Equal(expectedJobRequiredSkill.SkillId, result.SkillId);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(jobRequiredSkillId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<JobRequiredSkillResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetJobRequiredSkill_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var jobRequiredSkillId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(jobRequiredSkillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobRequiredSkill?)null);

            // Act & Assert
            await Assert.ThrowsAsync<JobRequiredSkillNotFoundException>(() =>
                _getHandler.Handle(new GetJobRequiredSkillRequest(jobRequiredSkillId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(jobRequiredSkillId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchJobRequiredSkills_ReturnsPagedJobRequiredSkillResponse()
        {
            // Arrange
            var request = new SearchJobRequiredSkillCommand
            {
                JobId = Guid.NewGuid(),
                PageNumber = 1,
                PageSize = 10
            };

            var jobRequiredSkills = new List<JobRequiredSkillResponse>
            {
                new JobRequiredSkillResponse(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                ),
                new JobRequiredSkillResponse(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                )
            };
            var totalCount = jobRequiredSkills.Count;

            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchJobRequiredSkillSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(jobRequiredSkills);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchJobRequiredSkillSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchJobRequiredSkillSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchJobRequiredSkillSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateJobRequiredSkill_ReturnsUpdatedJobRequiredSkillResponse()
        {
            // Arrange
            var existingJobRequiredSkill = JobRequiredSkill.Create(Guid.NewGuid(), Guid.NewGuid());
            var jobRequiredSkillId = existingJobRequiredSkill.Id;
            var request = new UpdateJobRequiredSkillCommand(
                jobRequiredSkillId,
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobRequiredSkillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingJobRequiredSkill);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(jobRequiredSkillId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobRequiredSkillId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<JobRequiredSkill>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateJobRequiredSkill_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var jobRequiredSkillId = Guid.NewGuid();
            var request = new UpdateJobRequiredSkillCommand(jobRequiredSkillId, Guid.NewGuid(), Guid.NewGuid());

            _repositoryMock.Setup(repo => repo.GetByIdAsync(jobRequiredSkillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobRequiredSkill?)null);

            // Act & Assert
            await Assert.ThrowsAsync<JobRequiredSkillNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jobRequiredSkillId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}