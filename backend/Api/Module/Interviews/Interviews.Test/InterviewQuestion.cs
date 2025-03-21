using Moq;
using Xunit;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Create.v1;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Delete.v1;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Get.v1;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Search.v1;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Update.v1;
using TalentMesh.Module.Interviews.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
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
    public class InterviewQuestionHandlerTests
    {
        private readonly Mock<IRepository<InterviewQuestion>> _repositoryMock;
        private readonly Mock<IReadRepository<InterviewQuestion>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateInterviewQuestionHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteInterviewQuestionHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetInterviewQuestionHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchInterviewQuestionsHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateInterviewQuestionHandler>> _updateLoggerMock;

        private readonly CreateInterviewQuestionHandler _createHandler;
        private readonly DeleteInterviewQuestionHandler _deleteHandler;
        private readonly GetInterviewQuestionHandler _getHandler;
        private readonly SearchInterviewQuestionsHandler _searchHandler;
        private readonly UpdateInterviewQuestionHandler _updateHandler;

        public InterviewQuestionHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<InterviewQuestion>>();
            _readRepositoryMock = new Mock<IReadRepository<InterviewQuestion>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateInterviewQuestionHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteInterviewQuestionHandler>>();
            _getLoggerMock = new Mock<ILogger<GetInterviewQuestionHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchInterviewQuestionsHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateInterviewQuestionHandler>>();

            _createHandler = new CreateInterviewQuestionHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteInterviewQuestionHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetInterviewQuestionHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchInterviewQuestionsHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateInterviewQuestionHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateInterviewQuestion_ReturnsInterviewQuestionResponse()
        {
            // Arrange
            var rubricId = Guid.NewGuid();
            var interviewId = Guid.NewGuid();
            var questionText = "Sync vs Async";
            var request = new CreateInterviewQuestionCommand(rubricId, interviewId, questionText);
            var expectedInterviewQuestion = InterviewQuestion.Create(request.RubricId!, request.InterviewId!, request.QuestionText);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<InterviewQuestion>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedInterviewQuestion);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<InterviewQuestion>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewQuestion_DeletesSuccessfully()
        {
            // Arrange
            var existingInterviewQuestion = InterviewQuestion.Create(Guid.NewGuid(), Guid.NewGuid(), "Sync vs Async");
            var InterviewQuestionId = existingInterviewQuestion.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewQuestionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInterviewQuestion);

            // Act
            await _deleteHandler.Handle(new DeleteInterviewQuestionCommand(InterviewQuestionId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingInterviewQuestion, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewQuestionId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewQuestion_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewQuestionId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewQuestionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewQuestion)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewQuestionNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteInterviewQuestionCommand(InterviewQuestionId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewQuestionId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewQuestion_ReturnsInterviewQuestionResponse()
        {
            // Arrange
            var expectedInterviewQuestion = InterviewQuestion.Create(Guid.NewGuid(), Guid.NewGuid(), "Sync vs Async");
            var InterviewQuestionId = expectedInterviewQuestion.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(InterviewQuestionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedInterviewQuestion);

            _cacheServiceMock.Setup(cache => cache.GetAsync<InterviewQuestionResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewQuestionResponse)null);

            // Act
            var result = await _getHandler.Handle(new GetInterviewQuestionRequest(InterviewQuestionId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedInterviewQuestion.Id, result.Id);
            Assert.Equal(expectedInterviewQuestion.RubricId, result.RubricId);
            Assert.Equal(expectedInterviewQuestion.InterviewId, result.InterviewId);
            Assert.Equal(expectedInterviewQuestion.QuestionText, result.QuestionText);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(InterviewQuestionId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<InterviewQuestionResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewQuestion_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewQuestionId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(InterviewQuestionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewQuestion)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewQuestionNotFoundException>(() =>
                _getHandler.Handle(new GetInterviewQuestionRequest(InterviewQuestionId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(InterviewQuestionId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchInterviewQuestions_ReturnsPagedInterviewQuestionResponse()
        {
            // Arrange
            var request = new SearchInterviewQuestionsCommand
            {
                RubricId = Guid.NewGuid(),
                PageNumber = 1,
                PageSize = 10
            };

            // Create domain entities (InterviewQuestion), not DTOs
            var InterviewQuestion1 = InterviewQuestion.Create(Guid.NewGuid(), Guid.NewGuid(), "Sync vs Async");
            var InterviewQuestion2 = InterviewQuestion.Create(Guid.NewGuid(), Guid.NewGuid(), "Sync vs Async");
            var InterviewQuestions = new List<InterviewQuestionResponse>
            {
                new InterviewQuestionResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Sync vs Async"),
                new InterviewQuestionResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Sync vs Async")
            };
            var totalCount = InterviewQuestions.Count;

            // Mock returns List<InterviewQuestion> (domain entities)
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchInterviewQuestionSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(InterviewQuestions);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchInterviewQuestionSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.QuestionText == "Sync vs Async"
            );

            Assert.Contains(result.Items, item =>
                item.QuestionText == "Sync vs Async"
            );

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchInterviewQuestionSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchInterviewQuestionSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Fact]
        public async Task UpdateInterviewQuestion_ReturnsUpdatedInterviewQuestionResponse()
        {
            // Arrange
            var existingInterviewQuestion = InterviewQuestion.Create(Guid.NewGuid(), Guid.NewGuid(), "Sync vs Async");
            var InterviewQuestionId = existingInterviewQuestion.Id;
            var request = new UpdateInterviewQuestionCommand(
                InterviewQuestionId,
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Sync vs Asybc"
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewQuestionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInterviewQuestion);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(InterviewQuestionId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewQuestionId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<InterviewQuestion>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewQuestion_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewQuestionId = Guid.NewGuid();
            var request = new UpdateInterviewQuestionCommand(InterviewQuestionId, Guid.NewGuid(), Guid.NewGuid(), "Sync vs Async");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewQuestionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewQuestion)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewQuestionNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewQuestionId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
