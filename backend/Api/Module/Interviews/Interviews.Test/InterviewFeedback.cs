using Moq;
using Xunit;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Create.v1;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Delete.v1;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Get.v1;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Search.v1;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Update.v1;
using TalentMesh.Module.Interviews.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Interviews.Tests
{
    public class InterviewFeedbackHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public InterviewFeedbackHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateInterviewFeedback_ReturnsInterviewFeedbackResponse()
        {
            var request = new CreateInterviewFeedbackCommand(Guid.NewGuid(), Guid.NewGuid(), "Sample Response", 85.5m);
            var expectedId = Guid.NewGuid();
            var response = new CreateInterviewFeedbackResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateInterviewFeedbackResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateInterviewFeedbackCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewFeedback_DeletesSuccessfully()
        {
            var InterviewFeedbackId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteInterviewFeedbackCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mediatorMock.Object.Send(new DeleteInterviewFeedbackCommand(InterviewFeedbackId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteInterviewFeedbackCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewFeedback_ThrowsExceptionIfNotFound()
        {
            var InterviewFeedbackId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteInterviewFeedbackCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewFeedbackNotFoundException(InterviewFeedbackId));

            var exception = await Assert.ThrowsAsync<InterviewFeedbackNotFoundException>(() => _mediatorMock.Object.Send(new DeleteInterviewFeedbackCommand(InterviewFeedbackId)));

            Assert.NotNull(exception);
            Assert.IsType<InterviewFeedbackNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteInterviewFeedbackCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewFeedback_ReturnsInterviewFeedbackResponse()
        {
            var InterviewFeedbackId = Guid.NewGuid();
            var InterviewQuestionId = Guid.NewGuid();
            var InterviewId = Guid.NewGuid();
            var response = "Sample Response";
            var score = 90.5m;

            var InterviewFeedbackResponse = new InterviewFeedbackResponse(InterviewFeedbackId, InterviewId, InterviewQuestionId, response, score);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInterviewFeedbackRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(InterviewFeedbackResponse);

            var result = await _mediatorMock.Object.Send(new GetInterviewFeedbackRequest(InterviewFeedbackId));

            Assert.NotNull(result);
            Assert.Equal(InterviewId, result.InterviewId);
            Assert.Equal(InterviewQuestionId, result.InterviewQuestionId);
            Assert.Equal(response, result.Response);
            Assert.Equal(score, result.Score);
            Assert.IsType<InterviewFeedbackResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetInterviewFeedbackRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewFeedback_ThrowsExceptionIfNotFound()
        {
            var InterviewFeedbackId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInterviewFeedbackRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewFeedbackNotFoundException(InterviewFeedbackId));

            var exception = await Assert.ThrowsAsync<InterviewFeedbackNotFoundException>(() => _mediatorMock.Object.Send(new GetInterviewFeedbackRequest(InterviewFeedbackId)));

            Assert.NotNull(exception);
            Assert.IsType<InterviewFeedbackNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetInterviewFeedbackRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchInterviewFeedbacks_ReturnsPagedInterviewFeedbackResponse()
        {
            var request = new SearchInterviewFeedbacksCommand
            {
                InterviewId = Guid.NewGuid(),
                InterviewQuestionId = Guid.NewGuid(),
                Response = "Sample Response",
                Score = 85.5m,
                PageNumber = 1,
                PageSize = 10
            };

            var pagedList = new PagedList<InterviewFeedbackResponse>(
                new[]
                {
                    new InterviewFeedbackResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Sample Response", 85.5m),
                    new InterviewFeedbackResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Final Response", 90.0m)
                },
                1,
                10,
                2);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchInterviewFeedbacksCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.All(result.Items, item => Assert.NotNull(item.Response));
            Assert.IsType<PagedList<InterviewFeedbackResponse>>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchInterviewFeedbacksCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewFeedback_ReturnsUpdatedInterviewFeedbackResponse()
        {
            var InterviewFeedbackId = Guid.NewGuid();
            var InterviewQuestionId = Guid.NewGuid();
            var InterviewId = Guid.NewGuid();
            var response = "Updated Response";
            var score = 95.5m;

            var request = new UpdateInterviewFeedbackCommand(InterviewId, InterviewFeedbackId, InterviewQuestionId, response, score);
            var responseUpdate = new UpdateInterviewFeedbackResponse(InterviewFeedbackId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInterviewFeedbackCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(responseUpdate);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(InterviewFeedbackId, result.Id);
            Assert.IsType<UpdateInterviewFeedbackResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateInterviewFeedbackCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewFeedback_ThrowsExceptionIfNotFound()
        {
            var InterviewFeedbackId = Guid.NewGuid();
            var InterviewQuestionId = Guid.NewGuid();
            var InterviewId = Guid.NewGuid();

            var request = new UpdateInterviewFeedbackCommand(InterviewFeedbackId, InterviewId, InterviewQuestionId, "Updated Response", 88.0m);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInterviewFeedbackCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewFeedbackNotFoundException(InterviewFeedbackId));

            var exception = await Assert.ThrowsAsync<InterviewFeedbackNotFoundException>(() => _mediatorMock.Object.Send(request));

            Assert.NotNull(exception);
            Assert.IsType<InterviewFeedbackNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateInterviewFeedbackCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
