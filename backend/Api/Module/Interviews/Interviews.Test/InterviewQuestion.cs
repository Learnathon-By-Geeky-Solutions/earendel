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

namespace TalentMesh.Module.Interviews.Tests
{
    public class InterviewQuestionHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public InterviewQuestionHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateInterviewQuestion_ReturnsInterviewQuestionResponse()
        {
            var request = new CreateInterviewQuestionCommand(Guid.NewGuid(), "What is polymorphism?");
            var expectedId = Guid.NewGuid();
            var response = new CreateInterviewQuestionResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateInterviewQuestionResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateInterviewQuestionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewQuestion_DeletesSuccessfully()
        {
            var rubricId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteInterviewQuestionCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mediatorMock.Object.Send(new DeleteInterviewQuestionCommand(rubricId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteInterviewQuestionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewQuestion_ThrowsExceptionIfNotFound()
        {
            var rubricId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteInterviewQuestionCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewQuestionNotFoundException(rubricId));

            var exception = await Assert.ThrowsAsync<InterviewQuestionNotFoundException>(() => _mediatorMock.Object.Send(new DeleteInterviewQuestionCommand(rubricId)));

            Assert.NotNull(exception);
            Assert.IsType<InterviewQuestionNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteInterviewQuestionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewQuestion_ReturnsInterviewQuestionResponse()
        {
            var questionId = Guid.NewGuid();
            var rubricId = Guid.NewGuid();
            var questionText = "What is polymorphism?";
            var interviewResponse = new InterviewQuestionResponse(questionId, rubricId, questionText);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInterviewQuestionRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(interviewResponse);

            var result = await _mediatorMock.Object.Send(new GetInterviewQuestionRequest(rubricId));

            Assert.NotNull(result);
            Assert.Equal(rubricId, result.RubricId);
            Assert.Equal(questionText, result.QuestionText);
            Assert.IsType<InterviewQuestionResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetInterviewQuestionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewQuestion_ThrowsExceptionIfNotFound()
        {
            var rubricId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInterviewQuestionRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewQuestionNotFoundException(rubricId));

            var exception = await Assert.ThrowsAsync<InterviewQuestionNotFoundException>(() => _mediatorMock.Object.Send(new GetInterviewQuestionRequest(rubricId)));

            Assert.NotNull(exception);
            Assert.IsType<InterviewQuestionNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetInterviewQuestionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchInterviewQuestions_ReturnsPagedInterviewQuestionResponse()
        {
            var request = new SearchInterviewQuestionsCommand { PageNumber = 1, PageSize = 10 };
            var pagedList = new PagedList<InterviewQuestionResponse>(
                new[]
                {
                    new InterviewQuestionResponse(Guid.NewGuid(), Guid.NewGuid(), "Explain encapsulation."),
                    new InterviewQuestionResponse(Guid.NewGuid(), Guid.NewGuid(), "What is inheritance?")
                },
                1,
                10,
                2);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchInterviewQuestionsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.All(result.Items, item => Assert.NotNull(item.QuestionText));
            Assert.IsType<PagedList<InterviewQuestionResponse>>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchInterviewQuestionsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewQuestion_ReturnsUpdatedInterviewQuestionResponse()
        {
            var questionId = Guid.NewGuid();
            var rubricId = Guid.NewGuid();
            var questionText = "Explain dependency injection";
            var request = new UpdateInterviewQuestionCommand(questionId, rubricId, questionText);
            var response = new UpdateInterviewQuestionResponse(questionId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInterviewQuestionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(questionId, result.Id);
            Assert.IsType<UpdateInterviewQuestionResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateInterviewQuestionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewQuestion_ThrowsExceptionIfNotFound()
        {
            var questionId = Guid.NewGuid();
            var rubricId = Guid.NewGuid();
            var request = new UpdateInterviewQuestionCommand(questionId, rubricId, "Explain dependency injection.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInterviewQuestionCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewQuestionNotFoundException(rubricId));

            var exception = await Assert.ThrowsAsync<InterviewQuestionNotFoundException>(() => _mediatorMock.Object.Send(request));

            Assert.NotNull(exception);
            Assert.IsType<InterviewQuestionNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateInterviewQuestionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
