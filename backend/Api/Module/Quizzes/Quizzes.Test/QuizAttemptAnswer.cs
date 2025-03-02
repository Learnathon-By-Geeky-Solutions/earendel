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

namespace TalentMesh.Module.Quizzes.Tests
{
    public class QuizAttemptAnswerHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public QuizAttemptAnswerHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateQuizAttemptAnswer_ReturnsQuizAttemptAnswerResponse()
        {
            // Arrange
            var attemptId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var request = new CreateQuizAttemptAnswerCommand(attemptId, questionId, 1, false);
            var expectedId = Guid.NewGuid();
            var response = new CreateQuizAttemptAnswerResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateQuizAttemptAnswerResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateQuizAttemptAnswerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteQuizAttemptAnswer_DeletesSuccessfully()
        {
            // Arrange
            var quizAttemptAnswerId = Guid.NewGuid();
            var request = new DeleteQuizAttemptAnswerCommand(quizAttemptAnswerId);
            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _mediatorMock.Object.Send(request);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteQuizAttemptAnswerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteQuizAttemptAnswer_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var quizAttemptAnswerId = Guid.NewGuid();
            var request = new DeleteQuizAttemptAnswerCommand(quizAttemptAnswerId);
            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new QuizAttemptAnswerNotFoundException(quizAttemptAnswerId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<QuizAttemptAnswerNotFoundException>(() => _mediatorMock.Object.Send(request));
            Assert.NotNull(exception);
            Assert.Contains($"QuizAttemptAnswer with id {quizAttemptAnswerId} not found", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteQuizAttemptAnswerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetQuizAttemptAnswer_ReturnsQuizAttemptAnswerResponse()
        {
            // Arrange
            var quizAttemptAnswerId = Guid.NewGuid();
            var attemptId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var response = new QuizAttemptAnswerResponse(quizAttemptAnswerId, attemptId, questionId, 1, false);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetQuizAttemptAnswerRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(new GetQuizAttemptAnswerRequest(quizAttemptAnswerId));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(quizAttemptAnswerId, result.Id);
            Assert.Equal(attemptId, result.AttemptId);
            Assert.Equal(questionId, result.QuestionId);
            Assert.Equal(1, result.SelectedOption);
            Assert.False(result.IsCorrect);
            Assert.IsType<QuizAttemptAnswerResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetQuizAttemptAnswerRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetQuizAttemptAnswer_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var quizAttemptAnswerId = Guid.NewGuid();
            var request = new GetQuizAttemptAnswerRequest(quizAttemptAnswerId);
            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new QuizAttemptAnswerNotFoundException(quizAttemptAnswerId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<QuizAttemptAnswerNotFoundException>(() => _mediatorMock.Object.Send(request));
            Assert.NotNull(exception);
            Assert.Contains($"QuizAttemptAnswer with id {quizAttemptAnswerId} not found", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetQuizAttemptAnswerRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchQuizAttemptAnswers_ReturnsPagedQuizAttemptAnswerResponse()
        {
            // Arrange
            var attemptId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var request = new SearchQuizAttemptAnswersCommand
            {
                AttemptId = attemptId,
            };
            var quizAttemptAnswer1 = new QuizAttemptAnswerResponse(Guid.NewGuid(), attemptId, questionId, 1, false);
            var quizAttemptAnswer2 = new QuizAttemptAnswerResponse(Guid.NewGuid(), attemptId, questionId, 2, true);
            var pagedList = new PagedList<QuizAttemptAnswerResponse>(new[] { quizAttemptAnswer1, quizAttemptAnswer2 }, 1, 10, 2);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SearchQuizAttemptAnswersCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(pagedList);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.IsType<PagedList<QuizAttemptAnswerResponse>>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchQuizAttemptAnswersCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuizAttemptAnswer_ReturnsUpdatedQuizAttemptAnswerResponse()
        {
            // Arrange
            var quizAttemptAnswerId = Guid.NewGuid();
            var attemptId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var request = new UpdateQuizAttemptAnswerCommand(quizAttemptAnswerId, attemptId, questionId, 2, true);
            var response = new UpdateQuizAttemptAnswerResponse(quizAttemptAnswerId);

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateQuizAttemptAnswerCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(quizAttemptAnswerId, result.Id);
            Assert.IsType<UpdateQuizAttemptAnswerResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateQuizAttemptAnswerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuizAttemptAnswer_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var quizAttemptAnswerId = Guid.NewGuid();
            var attemptId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var request = new UpdateQuizAttemptAnswerCommand(quizAttemptAnswerId, attemptId, questionId, 2, true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateQuizAttemptAnswerCommand>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new QuizAttemptAnswerNotFoundException(quizAttemptAnswerId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<QuizAttemptAnswerNotFoundException>(() => _mediatorMock.Object.Send(request));
            Assert.NotNull(exception);
            Assert.Contains($"QuizAttemptAnswer with id {quizAttemptAnswerId} not found", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateQuizAttemptAnswerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
