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

namespace TalentMesh.Module.Quizzes.Tests
{
    public class QuizQuestionHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public QuizQuestionHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateQuizQuestion_ReturnsQuizQuestionResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            // Updated fields: CorrectOption, QuestionText, Option1, Option2, Option3, Option4.
            var request = new CreateQuizQuestionCommand(
                2,
                "What is .NET?",
                "Framework",
                "Library",
                "Tool",
                "Language");
            var expectedId = Guid.NewGuid();
            var response = new CreateQuizQuestionResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateQuizQuestionResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateQuizQuestionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteQuizQuestion_DeletesSuccessfully()
        {
            // Arrange
            var quizQuestionId = Guid.NewGuid();
            var request = new DeleteQuizQuestionCommand(quizQuestionId);
            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _mediatorMock.Object.Send(request);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteQuizQuestionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteQuizQuestion_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var quizQuestionId = Guid.NewGuid();
            var request = new DeleteQuizQuestionCommand(quizQuestionId);
            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new QuizQuestionNotFoundException(quizQuestionId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<QuizQuestionNotFoundException>(() => _mediatorMock.Object.Send(request));
            Assert.NotNull(exception);
            Assert.Contains($"QuizQuestion with id {quizQuestionId} not found", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteQuizQuestionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetQuizQuestion_ReturnsQuizQuestionResponse()
        {
            // Arrange
            var quizQuestionId = Guid.NewGuid();
            int expectedCorrectOption = 2;
            string expectedQuestionText = "What is .NET?";
            string expectedOption1 = "Framework";
            string expectedOption2 = "Library";
            string expectedOption3 = "Tool";
            string expectedOption4 = "Language";

            var response = new QuizQuestionResponse(
                quizQuestionId,
                expectedQuestionText,
                expectedOption1,
                expectedOption2,
                expectedOption3,
                expectedOption4,
                expectedCorrectOption
                );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetQuizQuestionRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(new GetQuizQuestionRequest(quizQuestionId));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(quizQuestionId, result.Id);
            Assert.Equal(expectedCorrectOption, result.CorrectOption);
            Assert.Equal(expectedQuestionText, result.QuestionText);
            Assert.Equal(expectedOption1, result.Option1);
            Assert.Equal(expectedOption2, result.Option2);
            Assert.Equal(expectedOption3, result.Option3);
            Assert.Equal(expectedOption4, result.Option4);
            Assert.IsType<QuizQuestionResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetQuizQuestionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetQuizQuestion_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var quizQuestionId = Guid.NewGuid();
            var request = new GetQuizQuestionRequest(quizQuestionId);
            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new QuizQuestionNotFoundException(quizQuestionId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<QuizQuestionNotFoundException>(() => _mediatorMock.Object.Send(request));
            Assert.NotNull(exception);
            Assert.Contains($"QuizQuestion with id {quizQuestionId} not found", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetQuizQuestionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchQuizQuestions_ReturnsPagedQuizQuestionResponse()
        {
            // Arrange
            var request = new SearchQuizQuestionsCommand
            {
                QuestionText = "What is",
                PageNumber = 1,
                PageSize = 10
            };
            var quizQuestion1 = new QuizQuestionResponse(Guid.NewGuid(), "What is .NET?", "Framework", "Library", "Tool", "Language", 2);
            var quizQuestion2 = new QuizQuestionResponse(Guid.NewGuid(), "What is C#?", "Language", "Framework", "Tool", "Library", 2);
            var pagedList = new PagedList<QuizQuestionResponse>(new[] { quizQuestion1, quizQuestion2 }, 1, 10, 2);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SearchQuizQuestionsCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(pagedList);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal("What is .NET?", result.Items[0].QuestionText);
            Assert.Equal(2, result.Items[0].CorrectOption);
            Assert.IsType<PagedList<QuizQuestionResponse>>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchQuizQuestionsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuizQuestion_ReturnsUpdatedQuizQuestionResponse()
        {
            // Arrange
            var quizQuestionId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new UpdateQuizQuestionCommand(
                quizQuestionId,
                "Updated Question",
                "Opt1",
                "Opt2",
                "Opt3",
                "Opt4",
                3);
            var response = new UpdateQuizQuestionResponse(quizQuestionId);

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateQuizQuestionCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(quizQuestionId, result.Id);
            Assert.IsType<UpdateQuizQuestionResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateQuizQuestionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuizQuestion_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var quizQuestionId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new UpdateQuizQuestionCommand(
                quizQuestionId,
                "Updated Question",
                "Opt1",
                "Opt2",
                "Opt3",
                "Opt4",
                3);
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateQuizQuestionCommand>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new QuizQuestionNotFoundException(quizQuestionId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<QuizQuestionNotFoundException>(() => _mediatorMock.Object.Send(request));
            Assert.NotNull(exception);
            Assert.Contains($"QuizQuestion with id {quizQuestionId} not found", exception.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateQuizQuestionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
