using Moq;
using Xunit;
using TalentMesh.Module.Interviews.Application.Interviews.Create.v1;
using TalentMesh.Module.Interviews.Application.Interviews.Delete.v1;
using TalentMesh.Module.Interviews.Application.Interviews.Get.v1;
using TalentMesh.Module.Interviews.Application.Interviews.Search.v1;
using TalentMesh.Module.Interviews.Application.Interviews.Update.v1;
using TalentMesh.Module.Interviews.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Interviews.Tests
{
    public class InterviewHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public InterviewHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateInterview_ReturnsInterviewResponse()
        {
            var request = new CreateInterviewCommand(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Scheduled", "Initial interview", "meeting-123");
            var expectedId = Guid.NewGuid();
            var response = new CreateInterviewResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateInterviewResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateInterviewCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterview_DeletesSuccessfully()
        {
            var InterviewId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteInterviewCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mediatorMock.Object.Send(new DeleteInterviewCommand(InterviewId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteInterviewCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterview_ThrowsExceptionIfNotFound()
        {
            var interviewId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteInterviewCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewNotFoundException(interviewId));

            var exception = await Assert.ThrowsAsync<InterviewNotFoundException>(() => _mediatorMock.Object.Send(new DeleteInterviewCommand(interviewId)));

            Assert.NotNull(exception);
            Assert.IsType<InterviewNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteInterviewCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterview_ReturnsInterviewResponse()
        {
            var InterviewId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();
            var interviewerId = Guid.NewGuid();
            var interviewDate = DateTime.UtcNow;
            var status = "Scheduled";
            var notes = "Initial interview";
            var meetingId = "meeting-123";
            var InterviewResponse = new InterviewResponse(InterviewId, applicationId, interviewerId, interviewDate, status, notes, meetingId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInterviewRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(InterviewResponse);

            var result = await _mediatorMock.Object.Send(new GetInterviewRequest(InterviewId));

            Assert.NotNull(result);
            Assert.Equal(InterviewId, result.Id);
            Assert.Equal(applicationId, result.ApplicationId);
            Assert.Equal(interviewerId, result.InterviewerId);
            Assert.Equal(interviewDate, result.InterviewDate);
            Assert.Equal(status, result.Status);
            Assert.Equal(notes, result.Notes);
            Assert.Equal(meetingId, result.MeetingId);
            Assert.IsType<InterviewResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetInterviewRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterview_ThrowsExceptionIfNotFound()
        {
            var interviewId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInterviewRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewNotFoundException(interviewId));

            var exception = await Assert.ThrowsAsync<InterviewNotFoundException>(() => _mediatorMock.Object.Send(new GetInterviewRequest(interviewId)));

            Assert.NotNull(exception);
            Assert.IsType<InterviewNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetInterviewRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchInterviews_ReturnsPagedInterviewResponse()
        {
            var request = new SearchInterviewsCommand
            {
                ApplicationId = Guid.NewGuid(),
                InterviewerId = Guid.NewGuid(),
                Status = "Scheduled",
                PageNumber = 1,
                PageSize = 10
            };

            var pagedList = new PagedList<InterviewResponse>(
                new[]
                {
                    new InterviewResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Scheduled", "Initial interview", "meeting-123"),
                    new InterviewResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Completed", "Final round", "meeting-456")
                },
                1,
                10,
                2);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchInterviewsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.All(result.Items, item => Assert.NotNull(item.Status));
            Assert.IsType<PagedList<InterviewResponse>>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchInterviewsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterview_ReturnsUpdatedInterviewResponse()
        {
            var InterviewId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();
            var interviewerId = Guid.NewGuid();
            var request = new UpdateInterviewCommand(InterviewId, applicationId, interviewerId, DateTime.UtcNow, "Rescheduled", "Updated notes", "meeting-789");
            var response = new UpdateInterviewResponse(InterviewId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInterviewCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(InterviewId, result.Id);
            Assert.IsType<UpdateInterviewResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateInterviewCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterview_ThrowsExceptionIfNotFound()
        {
            var interviewId = Guid.NewGuid();
            var request = new UpdateInterviewCommand(interviewId, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Rescheduled", "Updated notes", "meeting-456");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInterviewCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewNotFoundException(interviewId));

            var exception = await Assert.ThrowsAsync<InterviewNotFoundException>(() => _mediatorMock.Object.Send(request));

            Assert.NotNull(exception);
            Assert.IsType<InterviewNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateInterviewCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
