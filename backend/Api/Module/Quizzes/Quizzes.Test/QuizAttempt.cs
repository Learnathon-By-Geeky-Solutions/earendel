using Moq;
using Xunit;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Create.v1;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Delete.v1;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Get.v1;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Search.v1;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Update.v1;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Framework.Core.Paging;
using MediatR;
using TalentMesh.Module.Quizzes.Domain;
using TalentMesh.Module.Quizzes.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Quizzes.Tests
{
    public class QuizAttemptHandlerTests
    {
        private readonly Mock<IRepository<QuizAttempt>> _repositoryMock;
        private readonly Mock<IReadRepository<QuizAttempt>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateQuizAttemptHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteQuizAttemptHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetQuizAttemptHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchQuizAttemptsHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateQuizAttemptHandler>> _updateLoggerMock;

        private readonly CreateQuizAttemptHandler _createHandler;
        private readonly DeleteQuizAttemptHandler _deleteHandler;
        private readonly GetQuizAttemptHandler _getHandler;
        private readonly SearchQuizAttemptsHandler _searchHandler;
        private readonly UpdateQuizAttemptHandler _updateHandler;

        public QuizAttemptHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<QuizAttempt>>();
            _readRepositoryMock = new Mock<IReadRepository<QuizAttempt>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateQuizAttemptHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteQuizAttemptHandler>>();
            _getLoggerMock = new Mock<ILogger<GetQuizAttemptHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchQuizAttemptsHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateQuizAttemptHandler>>();

            _createHandler = new CreateQuizAttemptHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteQuizAttemptHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetQuizAttemptHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchQuizAttemptsHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateQuizAttemptHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateQuizAttempt_ReturnsQuizAttemptResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var quizId = Guid.NewGuid();
            var request = new CreateQuizAttemptCommand(userId, 2, 0.8m);
            var expectedQuizAttempt = QuizAttempt.Create(request.UserId!, request.Score, request.TotalQuestions!);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<QuizAttempt>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedQuizAttempt);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<QuizAttempt>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteQuizAttempt_DeletesSuccessfully()
        {
            // Arrange
            var existingQuizAttempt = QuizAttempt.Create(Guid.NewGuid(), 0.8m, 2);
            var quizAttemptId = existingQuizAttempt.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(quizAttemptId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingQuizAttempt);

            // Act
            await _deleteHandler.Handle(new DeleteQuizAttemptCommand(quizAttemptId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingQuizAttempt, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(quizAttemptId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteQuizAttempt_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var quizAttemptId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(quizAttemptId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuizAttempt)null);

            // Act & Assert
            await Assert.ThrowsAsync<QuizAttemptNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteQuizAttemptCommand(quizAttemptId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(quizAttemptId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetQuizAttempt_ReturnsQuizAttemptResponse()
        {
            // Arrange
            var expectedQuizAttempt = QuizAttempt.Create(Guid.NewGuid(), 0.8m, 2);
            var quizAttemptId = expectedQuizAttempt.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(quizAttemptId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedQuizAttempt);

            _cacheServiceMock.Setup(cache => cache.GetAsync<QuizAttemptResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuizAttemptResponse)null);

            // Act
            var result = await _getHandler.Handle(new GetQuizAttemptRequest(quizAttemptId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedQuizAttempt.Id, result.Id);
            Assert.Equal(expectedQuizAttempt.UserId, result.UserId);
            Assert.Equal(expectedQuizAttempt.TotalQuestions, result.TotalQuestions);
            Assert.Equal(expectedQuizAttempt.Score, result.Score);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(quizAttemptId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<QuizAttemptResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetQuizAttempt_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var quizAttemptId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(quizAttemptId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuizAttempt)null);

            // Act & Assert
            await Assert.ThrowsAsync<QuizAttemptNotFoundException>(() =>
                _getHandler.Handle(new GetQuizAttemptRequest(quizAttemptId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(quizAttemptId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchQuizAttempts_ReturnsPagedQuizAttemptResponse()
        {
            // Arrange
            var request = new SearchQuizAttemptsCommand
            {
                TotalQuestions = 2,
                Score = 0.8m,
                PageSize = 10
            };

            var quizAttempts = new List<QuizAttemptResponse>
            {
                new QuizAttemptResponse(Guid.NewGuid(), Guid.NewGuid(), 0.8m, 2),
                new QuizAttemptResponse(Guid.NewGuid(), Guid.NewGuid(), 0.9m, 3)
            };
            var totalCount = quizAttempts.Count;

            // Mock returns List<QuizAttempt> (domain entities)
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchQuizAttemptSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(quizAttempts);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchQuizAttemptSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.TotalQuestions == 2 &&
                item.Score == 0.8m
            );

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchQuizAttemptSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchQuizAttemptSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Fact]
        public async Task UpdateQuizAttempt_ReturnsUpdatedQuizAttemptResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingQuizAttempt = QuizAttempt.Create(userId, 0.5m, 2);
            var quizAttemptId = existingQuizAttempt.Id;
            var request = new UpdateQuizAttemptCommand(
                quizAttemptId,
                userId,
                1.0m,
                2
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(quizAttemptId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingQuizAttempt);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(quizAttemptId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(quizAttemptId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<QuizAttempt>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuizAttempt_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var quizAttemptId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new UpdateQuizAttemptCommand(quizAttemptId, userId, 1.0m, 2);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(quizAttemptId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuizAttempt)null);

            // Act & Assert
            await Assert.ThrowsAsync<QuizAttemptNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(quizAttemptId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
