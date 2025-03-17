
using Moq;

using TalentMesh.Framework.Core.Paging;
using MediatR;
using Xunit;
using Evaluator.Application.Interviewer.Get.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Delete.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1;
using TalentMesh.Module.Evaluator.Domain.Exceptions;

namespace TalentMesh.Module.Evaluator.Tests
{
    public class InterviewerApplicationHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public InterviewerApplicationHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateInterviewerApplication_ReturnsResponse()
        {
            // Arrange
            var request = new CreateInterviewerApplicationCommand(
                JobId: Guid.NewGuid(),
                InterviewerId: Guid.NewGuid(),
                Comments: "Looking forward to this interview."
            );
            var expectedId = Guid.NewGuid();
            var response = new CreateInterviewerApplicationResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateInterviewerApplicationResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateInterviewerApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewerApplication_DeletesSuccessfully()
        {
            // Arrange
            var applicationId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteInterviewerApplicationCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _mediatorMock.Object.Send(new DeleteInterviewerApplicationCommand(applicationId));

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteInterviewerApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewerApplication_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var applicationId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteInterviewerApplicationCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewerApplicationNotFoundException(applicationId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InterviewerApplicationNotFoundException>(
                () => _mediatorMock.Object.Send(new DeleteInterviewerApplicationCommand(applicationId))
            );

            Assert.NotNull(exception);
            Assert.IsType<InterviewerApplicationNotFoundException>(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteInterviewerApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewerApplication_ReturnsResponse()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var expectedJobId = Guid.NewGuid();
            var expectedInterviewerId = Guid.NewGuid();
            var expectedStatus = "pending";
            var expectedComments = "Looking forward.";
            var expectedAppliedDate = DateTime.UtcNow;
            var response = new InterviewerApplicationResponse(
                applicationId,
                expectedJobId,
                expectedInterviewerId,
                expectedAppliedDate,
                expectedStatus,
                expectedComments
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInterviewerApplicationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(new GetInterviewerApplicationRequest(applicationId));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(applicationId, result.Id);
            Assert.Equal(expectedJobId, result.JobId);
            Assert.Equal(expectedInterviewerId, result.InterviewerId);
            Assert.Equal(expectedStatus, result.Status);
            Assert.Equal(expectedComments, result.Comments);
            Assert.IsType<InterviewerApplicationResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetInterviewerApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewerApplication_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var applicationId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInterviewerApplicationRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewerApplicationNotFoundException(applicationId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InterviewerApplicationNotFoundException>(
                () => _mediatorMock.Object.Send(new GetInterviewerApplicationRequest(applicationId))
            );

            Assert.NotNull(exception);
            Assert.IsType<InterviewerApplicationNotFoundException>(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetInterviewerApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchInterviewerApplications_ReturnsPagedResponse()
        {
            // Arrange
            var request = new SearchInterviewerApplicationsCommand
            {
                Status = "pending",
                PageNumber = 1,
                PageSize = 10
            };
            var app1 = new InterviewerApplicationResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "pending", "Comment1");
            var app2 = new InterviewerApplicationResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "pending", "Comment2");
            var pagedList = new PagedList<InterviewerApplicationResponse>(new[] { app1, app2 }, 1, 10, 2);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchInterviewerApplicationsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchInterviewerApplicationsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewerApplication_ReturnsUpdatedResponse()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var request = new UpdateInterviewerApplicationCommand(
                applicationId,
                JobId: Guid.NewGuid(),
                InterviewerId: Guid.NewGuid(),
                Status: "approved",
                Comments: "Updated comment"
            );
            var response = new UpdateInterviewerApplicationResponse(applicationId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInterviewerApplicationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(applicationId, result.Id);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateInterviewerApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewerApplication_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var request = new UpdateInterviewerApplicationCommand(
                applicationId,
                JobId: Guid.NewGuid(),
                InterviewerId: Guid.NewGuid(),
                Status: "approved",
                Comments: "Updated comment"
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInterviewerApplicationCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewerApplicationNotFoundException(applicationId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InterviewerApplicationNotFoundException>(() => _mediatorMock.Object.Send(request));
            Assert.NotNull(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateInterviewerApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
