using Moq;
using Xunit;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Create.v1;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Delete.v1;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Get.v1;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Search.v1;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Update.v1;
using TalentMesh.Module.Interviews.Domain.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using TalentMesh.Framework.Core.Caching;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Interviews.Tests
{
    public class InterviewFeedbackHandlerTests
    {
        private readonly Mock<IRepository<InterviewFeedback>> _repositoryMock;
        private readonly Mock<IReadRepository<InterviewFeedback>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateInterviewFeedbackHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteInterviewFeedbackHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetInterviewFeedbackHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchInterviewFeedbacksHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateInterviewFeedbackHandler>> _updateLoggerMock;

        private readonly CreateInterviewFeedbackHandler _createHandler;
        private readonly DeleteInterviewFeedbackHandler _deleteHandler;
        private readonly GetInterviewFeedbackHandler _getHandler;
        private readonly SearchInterviewFeedbacksHandler _searchHandler;
        private readonly UpdateInterviewFeedbackHandler _updateHandler;

        public InterviewFeedbackHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<InterviewFeedback>>();
            _readRepositoryMock = new Mock<IReadRepository<InterviewFeedback>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateInterviewFeedbackHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteInterviewFeedbackHandler>>();
            _getLoggerMock = new Mock<ILogger<GetInterviewFeedbackHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchInterviewFeedbacksHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateInterviewFeedbackHandler>>();

            _createHandler = new CreateInterviewFeedbackHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteInterviewFeedbackHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetInterviewFeedbackHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchInterviewFeedbacksHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateInterviewFeedbackHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateInterviewFeedback_ReturnsInterviewFeedbackResponse()
        {
            // Arrange
            var interviewId = Guid.NewGuid();
            var interviewQuestionId = Guid.NewGuid();
            var response = "Good";
            var score = 8.0m;
            var request = new CreateInterviewFeedbackCommand(interviewId, interviewQuestionId, response, score);
            var expectedInterviewFeedback = InterviewFeedback.Create(request.InterviewId!, request.InterviewQuestionId!, request.Response, request.Score);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<InterviewFeedback>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedInterviewFeedback);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<InterviewFeedback>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewFeedback_DeletesSuccessfully()
        {
            // Arrange
            var existingInterviewFeedback = InterviewFeedback.Create(Guid.NewGuid(), Guid.NewGuid(), "Good", 0.8m);
            var InterviewFeedbackId = existingInterviewFeedback.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewFeedbackId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInterviewFeedback);

            // Act
            await _deleteHandler.Handle(new DeleteInterviewFeedbackCommand(InterviewFeedbackId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingInterviewFeedback, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewFeedbackId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewFeedback_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewFeedbackId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewFeedbackId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewFeedback?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewFeedbackNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteInterviewFeedbackCommand(InterviewFeedbackId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewFeedbackId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewFeedback_ReturnsInterviewFeedbackResponse()
        {
            // Arrange
            var expectedInterviewFeedback = InterviewFeedback.Create(Guid.NewGuid(), Guid.NewGuid(), "Good", 0.8m);
            var InterviewFeedbackId = expectedInterviewFeedback.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(InterviewFeedbackId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedInterviewFeedback);

            _cacheServiceMock.Setup(cache => cache.GetAsync<InterviewFeedbackResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewFeedbackResponse?)null);

            // Act
            var result = await _getHandler.Handle(new GetInterviewFeedbackRequest(InterviewFeedbackId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedInterviewFeedback.Id, result.Id);
            Assert.Equal(expectedInterviewFeedback.InterviewId, result.InterviewId);
            Assert.Equal(expectedInterviewFeedback.InterviewQuestionId, result.InterviewQuestionId);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(InterviewFeedbackId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<InterviewFeedbackResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewFeedback_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewFeedbackId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(InterviewFeedbackId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewFeedback?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewFeedbackNotFoundException>(() =>
                _getHandler.Handle(new GetInterviewFeedbackRequest(InterviewFeedbackId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(InterviewFeedbackId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchInterviewFeedbacks_ReturnsPagedInterviewFeedbackResponse()
        {
            // Arrange
            var request = new SearchInterviewFeedbacksCommand
            {
                InterviewId = Guid.NewGuid(),
                PageNumber = 1,
                PageSize = 10
            };

            var InterviewFeedbacks = new List<InterviewFeedbackResponse>
            {
                new InterviewFeedbackResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Good", 0.8m),
                new InterviewFeedbackResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Good", 0.9m),
            };
            var totalCount = InterviewFeedbacks.Count;

            // Mock returns List<InterviewFeedback> (domain entities)
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchInterviewFeedbackSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(InterviewFeedbacks);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchInterviewFeedbackSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.Response == "Good" &&
                item.Score == 0.8m
            );

            Assert.Contains(result.Items, item =>
                item.Response == "Good" &&
                item.Score == 0.9m
            );

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchInterviewFeedbackSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchInterviewFeedbackSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Fact]
        public async Task UpdateInterviewFeedback_ReturnsUpdatedInterviewFeedbackResponse()
        {
            // Arrange
            var existingInterviewFeedback = InterviewFeedback.Create(Guid.NewGuid(), Guid.NewGuid(), "Good", 0.5m);
            var InterviewFeedbackId = existingInterviewFeedback.Id;
            var request = new UpdateInterviewFeedbackCommand(
                InterviewFeedbackId,
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Bad",
                1.0m
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewFeedbackId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInterviewFeedback);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(InterviewFeedbackId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewFeedbackId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<InterviewFeedback>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewFeedback_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewFeedbackId = Guid.NewGuid();
            var request = new UpdateInterviewFeedbackCommand(InterviewFeedbackId, Guid.NewGuid(), Guid.NewGuid(), "Good", 1.0m);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewFeedbackId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewFeedback?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewFeedbackNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewFeedbackId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
