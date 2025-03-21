using Moq;
using TalentMesh.Framework.Core.Paging;
using MediatR;
using Xunit;
using Evaluator.Application.Interviewer.Get.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Delete.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1;
using TalentMesh.Module.Evaluator.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Evaluator.Domain;
using TalentMesh.Framework.Core.Caching;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Evaluator.Tests
{
    public class InterviewerApplicationHandlerTests
    {
        private readonly Mock<IRepository<InterviewerApplication>> _repositoryMock;
        private readonly Mock<IReadRepository<InterviewerApplication>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateInterviewerApplicationHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteInterviewerApplicationHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetInterviewerApplicationHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchInterviewerApplicationsHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateInterviewerApplicationHandler>> _updateLoggerMock;

        private readonly CreateInterviewerApplicationHandler _createHandler;
        private readonly DeleteInterviewerApplicationHandler _deleteHandler;
        private readonly GetInterviewerApplicationHandler _getHandler;
        private readonly SearchInterviewerApplicationsHandler _searchHandler;
        private readonly UpdateInterviewerApplicationHandler _updateHandler;

        public InterviewerApplicationHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<InterviewerApplication>>();
            _readRepositoryMock = new Mock<IReadRepository<InterviewerApplication>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateInterviewerApplicationHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteInterviewerApplicationHandler>>();
            _getLoggerMock = new Mock<ILogger<GetInterviewerApplicationHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchInterviewerApplicationsHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateInterviewerApplicationHandler>>();

            _createHandler = new CreateInterviewerApplicationHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteInterviewerApplicationHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetInterviewerApplicationHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchInterviewerApplicationsHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateInterviewerApplicationHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateInterviewerApplication_ReturnsInterviewerApplicationResponse()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var interviewerId = Guid.NewGuid();
            var comments = "no comments";
            var request = new CreateInterviewerApplicationCommand(jobId, interviewerId, comments);
            var expectedInterviewerApplication = InterviewerApplication.Create(request.JobId!, request.InterviewerId!, request.Comments);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<InterviewerApplication>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedInterviewerApplication);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<InterviewerApplication>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewerApplication_DeletesSuccessfully()
        {
            // Arrange
            var existingInterviewerApplication = InterviewerApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "no comments");
            var InterviewerApplicationId = existingInterviewerApplication.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerApplicationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInterviewerApplication);

            // Act
            await _deleteHandler.Handle(new DeleteInterviewerApplicationCommand(InterviewerApplicationId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingInterviewerApplication, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerApplicationId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewerApplication_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewerApplicationId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerApplicationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewerApplication)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewerApplicationNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteInterviewerApplicationCommand(InterviewerApplicationId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerApplicationId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewerApplication_ReturnsInterviewerApplicationResponse()
        {
            // Arrange
            var expectedInterviewerApplication = InterviewerApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "no comments");
            var InterviewerApplicationId = expectedInterviewerApplication.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerApplicationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedInterviewerApplication);

            _cacheServiceMock.Setup(cache => cache.GetAsync<InterviewerApplicationResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewerApplicationResponse)null);

            // Act
            var result = await _getHandler.Handle(new GetInterviewerApplicationRequest(InterviewerApplicationId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedInterviewerApplication.Id, result.Id);
            Assert.Equal(expectedInterviewerApplication.JobId, result.JobId);
            Assert.Equal(expectedInterviewerApplication.InterviewerId, result.InterviewerId);
            Assert.Equal(expectedInterviewerApplication.Comments, result.Comments);
            Assert.Equal(expectedInterviewerApplication.AppliedDate, result.AppliedDate);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerApplicationId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<InterviewerApplicationResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewerApplication_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewerApplicationId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerApplicationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewerApplication)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewerApplicationNotFoundException>(() =>
                _getHandler.Handle(new GetInterviewerApplicationRequest(InterviewerApplicationId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerApplicationId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchInterviewerApplications_ReturnsPagedInterviewerApplicationResponse()
        {
            // Arrange
            var request = new SearchInterviewerApplicationsCommand
            {
                Status = "pending",
                Comments = "no comments",
                PageNumber = 1,
                PageSize = 10
            };

            var InterviewerApplications = new List<InterviewerApplicationResponse>
            {
                new InterviewerApplicationResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "pending", "no comments"),
                new InterviewerApplicationResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "approved", "no comments")
            };
            var totalCount = InterviewerApplications.Count;

            // Mock returns List<InterviewerApplication> (domain entities)
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchInterviewerApplicationSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(InterviewerApplications);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchInterviewerApplicationSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.Status == "pending" &&
                item.Comments == "no comments"
            );

            Assert.Contains(result.Items, item =>
                item.Status == "approved" &&
                item.Comments == "no comments"
            );


            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchInterviewerApplicationSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchInterviewerApplicationSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Fact]
        public async Task UpdateInterviewerApplication_ReturnsUpdatedInterviewerApplicationResponse()
        {
            // Arrange
            var existingInterviewerApplication = InterviewerApplication.Create(Guid.NewGuid(), Guid.NewGuid(), "no comments");
            var InterviewerApplicationId = existingInterviewerApplication.Id;
            var jobId = Guid.NewGuid();
            var interviewerId = Guid.NewGuid();
            var request = new UpdateInterviewerApplicationCommand(
                InterviewerApplicationId,
                jobId,
                interviewerId,
                "rejected",
                "no comments"
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerApplicationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInterviewerApplication);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(InterviewerApplicationId, result.Id);
            

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerApplicationId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<InterviewerApplication>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewerApplication_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewerApplicationId = Guid.NewGuid();
            var request = new UpdateInterviewerApplicationCommand(InterviewerApplicationId, Guid.NewGuid(), Guid.NewGuid(), "approved", "no comments");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerApplicationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewerApplication)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewerApplicationNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerApplicationId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}
