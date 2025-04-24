using Moq;
using Xunit;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Create.v1;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Delete.v1;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Get.v1;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Search.v1;
using TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Update.v1;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Framework.Core.Paging;
using MediatR;
using TalentMesh.Module.Quizzes.Domain;
using TalentMesh.Module.Quizzes.Domain.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Quizzes.Tests
{
    public class QuizAttemptAnswerHandlerTests
    {
        private readonly Mock<IRepository<QuizAttemptAnswer>> _repositoryMock;
        private readonly Mock<IReadRepository<QuizAttemptAnswer>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateQuizAttemptAnswerHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteQuizAttemptAnswerHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetQuizAttemptAnswerHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchQuizAttemptAnswersHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateQuizAttemptAnswerHandler>> _updateLoggerMock;

        private readonly CreateQuizAttemptAnswerHandler _createHandler;
        private readonly DeleteQuizAttemptAnswerHandler _deleteHandler;
        private readonly GetQuizAttemptAnswerHandler _getHandler;
        private readonly SearchQuizAttemptAnswersHandler _searchHandler;
        private readonly UpdateQuizAttemptAnswerHandler _updateHandler;

        public QuizAttemptAnswerHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<QuizAttemptAnswer>>();
            _readRepositoryMock = new Mock<IReadRepository<QuizAttemptAnswer>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateQuizAttemptAnswerHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteQuizAttemptAnswerHandler>>();
            _getLoggerMock = new Mock<ILogger<GetQuizAttemptAnswerHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchQuizAttemptAnswersHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateQuizAttemptAnswerHandler>>();

            _createHandler = new CreateQuizAttemptAnswerHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteQuizAttemptAnswerHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetQuizAttemptAnswerHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchQuizAttemptAnswersHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateQuizAttemptAnswerHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateQuizAttemptAnswer_ReturnsQuizAttemptAnswerResponse()
        {
            // Arrange
            var attemptId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var request = new CreateQuizAttemptAnswerCommand(attemptId, questionId, 2, true);
            var expectedQuizAttemptAnswer = QuizAttemptAnswer.Create(request.AttemptId!, request.QuestionId!, 2, true);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<QuizAttemptAnswer>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedQuizAttemptAnswer);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<QuizAttemptAnswer>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteQuizAttemptAnswer_DeletesSuccessfully()
        {
            // Arrange
            var existingQuizAttemptAnswer = QuizAttemptAnswer.Create(Guid.NewGuid(), Guid.NewGuid(), 2, true);
            var QuizAttemptAnswerId = existingQuizAttemptAnswer.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(QuizAttemptAnswerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingQuizAttemptAnswer);

            // Act
            await _deleteHandler.Handle(new DeleteQuizAttemptAnswerCommand(QuizAttemptAnswerId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingQuizAttemptAnswer, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(QuizAttemptAnswerId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteQuizAttemptAnswer_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var QuizAttemptAnswerId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(QuizAttemptAnswerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuizAttemptAnswer?)null);

            // Act & Assert
            await Assert.ThrowsAsync<QuizAttemptAnswerNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteQuizAttemptAnswerCommand(QuizAttemptAnswerId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(QuizAttemptAnswerId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetQuizAttemptAnswer_ReturnsQuizAttemptAnswerResponse()
        {
            // Arrange
            var expectedQuizAttemptAnswer = QuizAttemptAnswer.Create(Guid.NewGuid(), Guid.NewGuid(), 2, true);
            var QuizAttemptAnswerId = expectedQuizAttemptAnswer.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(QuizAttemptAnswerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedQuizAttemptAnswer);

            _cacheServiceMock.Setup(cache => cache.GetAsync<QuizAttemptAnswerResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuizAttemptAnswerResponse?)null);

            // Act
            var result = await _getHandler.Handle(new GetQuizAttemptAnswerRequest(QuizAttemptAnswerId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedQuizAttemptAnswer.Id, result.Id);
            Assert.Equal(expectedQuizAttemptAnswer.AttemptId, result.AttemptId);
            Assert.Equal(expectedQuizAttemptAnswer.SelectedOption, result.SelectedOption);
            Assert.Equal(expectedQuizAttemptAnswer.IsCorrect, result.IsCorrect);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(QuizAttemptAnswerId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<QuizAttemptAnswerResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetQuizAttemptAnswer_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var QuizAttemptAnswerId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(QuizAttemptAnswerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuizAttemptAnswer?)null);

            // Act & Assert
            await Assert.ThrowsAsync<QuizAttemptAnswerNotFoundException>(() =>
                _getHandler.Handle(new GetQuizAttemptAnswerRequest(QuizAttemptAnswerId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(QuizAttemptAnswerId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchQuizAttemptAnswers_ReturnsPagedQuizAttemptAnswerResponse()
        {
            // Arrange
            var request = new SearchQuizAttemptAnswersCommand
            {
                AttemptId = Guid.NewGuid(),
                PageNumber = 1,
                PageSize = 10
            };

            // Create domain entities (QuizAttemptAnswer), not DTOs
            var QuizAttemptAnswer1 = QuizAttemptAnswer.Create(Guid.NewGuid(), Guid.NewGuid(), 2, true);
            var QuizAttemptAnswer2 = QuizAttemptAnswer.Create(Guid.NewGuid(), Guid.NewGuid(), 3, false);
            var QuizAttemptAnswers = new List<QuizAttemptAnswerResponse>
            {
                new QuizAttemptAnswerResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 2, true),
                new QuizAttemptAnswerResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 3, false)
            };
            var totalCount = QuizAttemptAnswers.Count;

            // Mock returns List<QuizAttemptAnswer> (domain entities)
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchQuizAttemptAnswerSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(QuizAttemptAnswers);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchQuizAttemptAnswerSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.SelectedOption == 2 &&
                item.IsCorrect == true
            );

            Assert.Contains(result.Items, item =>
                item.SelectedOption == 3 &&
                item.IsCorrect == false
            );

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchQuizAttemptAnswerSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchQuizAttemptAnswerSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Fact]
        public async Task UpdateQuizAttemptAnswer_ReturnsUpdatedQuizAttemptAnswerResponse()
        {
            // Arrange
            var existingQuizAttemptAnswer = QuizAttemptAnswer.Create(Guid.NewGuid(), Guid.NewGuid(), 2, true);
            var QuizAttemptAnswerId = existingQuizAttemptAnswer.Id;
            var request = new UpdateQuizAttemptAnswerCommand(
                QuizAttemptAnswerId,
                Guid.NewGuid(),
                Guid.NewGuid(),
                3,
                true
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(QuizAttemptAnswerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingQuizAttemptAnswer);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(QuizAttemptAnswerId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(QuizAttemptAnswerId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<QuizAttemptAnswer>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuizAttemptAnswer_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var QuizAttemptAnswerId = Guid.NewGuid();
            var request = new UpdateQuizAttemptAnswerCommand(QuizAttemptAnswerId, Guid.NewGuid(), Guid.NewGuid(), 2, false);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(QuizAttemptAnswerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuizAttemptAnswer?)null);

            // Act & Assert
            await Assert.ThrowsAsync<QuizAttemptAnswerNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(QuizAttemptAnswerId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}
