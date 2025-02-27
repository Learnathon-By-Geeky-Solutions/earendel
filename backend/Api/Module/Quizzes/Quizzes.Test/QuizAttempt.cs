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

namespace TalentMesh.Module.Quizzes.Tests
{
    public class QuizAttemptHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public QuizAttemptHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateQuizAttempt_ReturnsQuizAttemptResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new CreateQuizAttemptCommand(userId, 10, 8.5m);
            var expectedId = Guid.NewGuid();
            var response = new CreateQuizAttemptResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateQuizAttemptResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateQuizAttemptCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteQuizAttempt_DeletesSuccessfully()
        {
            // Arrange
            var quizAttemptId = Guid.NewGuid();
            var request = new DeleteQuizAttemptCommand(quizAttemptId);
            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _mediatorMock.Object.Send(request);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteQuizAttemptCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteQuizAttempt_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var quizAttemptId = Guid.NewGuid();
            var request = new DeleteQuizAttemptCommand(quizAttemptId);
            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new QuizAttemptNotFoundException(quizAttemptId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<QuizAttemptNotFoundException>(() => _mediatorMock.Object.Send(request));
            Assert.NotNull(exception);
            Assert.Contains($"QuizAttempt with id {quizAttemptId} not found", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteQuizAttemptCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetQuizAttempt_ReturnsQuizAttemptResponse()
        {
            // Arrange
            var quizAttemptId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            int expectedTotalQuestions = 10;
            decimal expectedScore = 8.5m;
            var response = new QuizAttemptResponse(quizAttemptId, userId, expectedScore, expectedTotalQuestions);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetQuizAttemptRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(new GetQuizAttemptRequest(quizAttemptId));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(quizAttemptId, result.Id);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(expectedTotalQuestions, result.TotalQuestions);
            Assert.Equal(expectedScore, result.Score);
            Assert.IsType<QuizAttemptResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetQuizAttemptRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetQuizAttempt_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var quizAttemptId = Guid.NewGuid();
            var request = new GetQuizAttemptRequest(quizAttemptId);
            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new QuizAttemptNotFoundException(quizAttemptId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<QuizAttemptNotFoundException>(() => _mediatorMock.Object.Send(request));
            Assert.NotNull(exception);
            Assert.Contains($"QuizAttempt with id {quizAttemptId} not found", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetQuizAttemptRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchQuizAttempts_ReturnsPagedQuizAttemptResponse()
        {
            // Arrange
            var request = new SearchQuizAttemptsCommand { TotalQuestions = 1, Score = 10 };
            var quizAttempt1 = new QuizAttemptResponse(Guid.NewGuid(), Guid.NewGuid(), 8.5m, 10);
            var quizAttempt2 = new QuizAttemptResponse(Guid.NewGuid(), Guid.NewGuid(), 12.0m, 15);
            var pagedList = new PagedList<QuizAttemptResponse>(new[] { quizAttempt1, quizAttempt2 }, 1, 10, 2);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SearchQuizAttemptsCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(pagedList);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.IsType<PagedList<QuizAttemptResponse>>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchQuizAttemptsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuizAttempt_ReturnsUpdatedQuizAttemptResponse()
        {
            // Arrange
            var quizAttemptId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new UpdateQuizAttemptCommand(quizAttemptId, userId, 9.0m, 12);
            var response = new UpdateQuizAttemptResponse(quizAttemptId);

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateQuizAttemptCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(quizAttemptId, result.Id);
            Assert.IsType<UpdateQuizAttemptResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateQuizAttemptCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuizAttempt_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var quizAttemptId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new UpdateQuizAttemptCommand(quizAttemptId, userId, 9.0m, 12);
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateQuizAttemptCommand>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new QuizAttemptNotFoundException(quizAttemptId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<QuizAttemptNotFoundException>(() => _mediatorMock.Object.Send(request));
            Assert.NotNull(exception);
            Assert.Contains($"QuizAttempt with id {quizAttemptId} not found", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateQuizAttemptCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
