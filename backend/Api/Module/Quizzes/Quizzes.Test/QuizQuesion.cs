using Moq;
using Xunit;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Create.v1;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Delete.v1;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Get.v1;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Search.v1;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Update.v1;
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
    public class QuizQuestionHandlerTests
    {
        private readonly Mock<IRepository<QuizQuestion>> _repositoryMock;
        private readonly Mock<IReadRepository<QuizQuestion>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateQuizQuestionHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteQuizQuestionHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetQuizQuestionHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchQuizQuestionsHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateQuizQuestionHandler>> _updateLoggerMock;

        private readonly CreateQuizQuestionHandler _createHandler;
        private readonly DeleteQuizQuestionHandler _deleteHandler;
        private readonly GetQuizQuestionHandler _getHandler;
        private readonly SearchQuizQuestionsHandler _searchHandler;
        private readonly UpdateQuizQuestionHandler _updateHandler;

        public QuizQuestionHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<QuizQuestion>>();
            _readRepositoryMock = new Mock<IReadRepository<QuizQuestion>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateQuizQuestionHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteQuizQuestionHandler>>();
            _getLoggerMock = new Mock<ILogger<GetQuizQuestionHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchQuizQuestionsHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateQuizQuestionHandler>>();

            _createHandler = new CreateQuizQuestionHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteQuizQuestionHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetQuizQuestionHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchQuizQuestionsHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateQuizQuestionHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateQuizQuestion_ReturnsQuizQuestionResponse()
        {
            // Arrange
            var subSkillId = Guid.NewGuid();
            var seniorityLevelId = Guid.NewGuid();
            var request = new CreateQuizQuestionCommand(2, "Effective C#", "C# advanced topics", "string1", "string2", "string3");
            var expectedQuizQuestion = QuizQuestion.Create(request.QuestionText!, request.Option1, request.Option2, request.Option3, request.Option4, 2);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<QuizQuestion>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedQuizQuestion);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<QuizQuestion>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteQuizQuestion_DeletesSuccessfully()
        {
            // Arrange
            var existingQuizQuestion = QuizQuestion.Create("Effective C#", "C# advanced topics", "string1", "string2", "string3", 2);
            var QuizQuestionId = existingQuizQuestion.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(QuizQuestionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingQuizQuestion);

            // Act
            await _deleteHandler.Handle(new DeleteQuizQuestionCommand(QuizQuestionId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingQuizQuestion, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(QuizQuestionId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteQuizQuestion_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var QuizQuestionId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(QuizQuestionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuizQuestion)null);

            // Act & Assert
            await Assert.ThrowsAsync<QuizQuestionNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteQuizQuestionCommand(QuizQuestionId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(QuizQuestionId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetQuizQuestion_ReturnsQuizQuestionResponse()
        {
            // Arrange
            var expectedQuizQuestion = QuizQuestion.Create("Effective C#", "C# advanced topics", "string1", "string2", "string3", 2);
            var QuizQuestionId = expectedQuizQuestion.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(QuizQuestionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedQuizQuestion);

            _cacheServiceMock.Setup(cache => cache.GetAsync<QuizQuestionResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuizQuestionResponse)null);

            // Act
            var result = await _getHandler.Handle(new GetQuizQuestionRequest(QuizQuestionId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedQuizQuestion.Id, result.Id);
            Assert.Equal(expectedQuizQuestion.QuestionText, result.QuestionText);
            Assert.Equal(expectedQuizQuestion.CorrectOption, result.CorrectOption);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(QuizQuestionId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<QuizQuestionResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetQuizQuestion_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var QuizQuestionId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(QuizQuestionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuizQuestion)null);

            // Act & Assert
            await Assert.ThrowsAsync<QuizQuestionNotFoundException>(() =>
                _getHandler.Handle(new GetQuizQuestionRequest(QuizQuestionId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(QuizQuestionId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchQuizQuestions_ReturnsPagedQuizQuestionResponse()
        {
            // Arrange
            var request = new SearchQuizQuestionsCommand
            {
                QuestionText = "Effective",
                PageNumber = 1,
                PageSize = 10
            };

            var QuizQuestions = new List<QuizQuestionResponse>
            {
                new QuizQuestionResponse(Guid.NewGuid(), "Effective C#", "C# advanced topics", "string1", "string2", "string3", 2),
                new QuizQuestionResponse(Guid.NewGuid(), "Effective Java", "Effective C#", "string1", "string2", "string3", 3)
            };
            var totalCount = QuizQuestions.Count;

            // Mock returns List<QuizQuestion> (domain entities)
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchQuizQuestionSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(QuizQuestions);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchQuizQuestionSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.QuestionText == "Effective C#" &&
                item.CorrectOption == 2
            );

            Assert.Contains(result.Items, item =>
                item.QuestionText == "Effective Java" &&
                item.CorrectOption == 3
            );

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchQuizQuestionSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchQuizQuestionSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Fact]
        public async Task UpdateQuizQuestion_ReturnsUpdatedQuizQuestionResponse()
        {
            // Arrange
            var existingQuizQuestion = QuizQuestion.Create("Effective C#", "C# advanced topics", "string1", "string2", "string3", 2);
            var QuizQuestionId = existingQuizQuestion.Id;
            var request = new UpdateQuizQuestionCommand(
                QuizQuestionId,
                "Updated Title",
                2
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(QuizQuestionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingQuizQuestion);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(QuizQuestionId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(QuizQuestionId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<QuizQuestion>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuizQuestion_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var QuizQuestionId = Guid.NewGuid();
            var request = new UpdateQuizQuestionCommand(QuizQuestionId, "Title", 3);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(QuizQuestionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuizQuestion)null);

            // Act & Assert
            await Assert.ThrowsAsync<QuizQuestionNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(QuizQuestionId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}
