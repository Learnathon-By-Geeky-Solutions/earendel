
using Moq;
using TalentMesh.Module.Interviews.Domain.Exceptions;
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
    public class InterviewerAvailabilityHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public InterviewerAvailabilityHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateInterviewerAvailability_ReturnsResponse()
        {
            // Arrange
            var request = new CreateInterviewerAvailabilityCommand(
                InterviewerId: Guid.NewGuid(),
                StartTime: DateTime.UtcNow.AddDays(1),
                EndTime: DateTime.UtcNow.AddDays(1).AddHours(2),
                IsAvailable: true
            );
            var expectedId = Guid.NewGuid();
            var response = new CreateInterviewerAvailabilityResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateInterviewerAvailabilityCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewerAvailability_DeletesSuccessfully()
        {
            var availabilityId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteInterviewerAvailabilityCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mediatorMock.Object.Send(new DeleteInterviewerAvailabilityCommand(availabilityId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteInterviewerAvailabilityCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewerAvailability_ThrowsExceptionIfNotFound()
        {
            var availabilityId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteInterviewerAvailabilityCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewerAvailabilityNotFoundException(availabilityId));

            var exception = await Assert.ThrowsAsync<InterviewerAvailabilityNotFoundException>(
                () => _mediatorMock.Object.Send(new DeleteInterviewerAvailabilityCommand(availabilityId))
            );

            Assert.NotNull(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteInterviewerAvailabilityCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewerAvailability_ReturnsResponse()
        {
            var availabilityId = Guid.NewGuid();
            var interviewerId = Guid.NewGuid();
            var startTime = DateTime.UtcNow.AddDays(1);
            var endTime = startTime.AddHours(2);
            var expectedIsAvailable = true;
            var response = new InterviewerAvailabilityResponse(availabilityId, interviewerId, startTime, endTime, expectedIsAvailable);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInterviewerAvailabilityRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(new GetInterviewerAvailabilityRequest(availabilityId));

            Assert.NotNull(result);
            Assert.Equal(availabilityId, result.Id);
            Assert.Equal(interviewerId, result.InterviewerId);
            Assert.Equal(expectedIsAvailable, result.IsAvailable);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetInterviewerAvailabilityRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewerAvailability_ThrowsExceptionIfNotFound()
        {
            var availabilityId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInterviewerAvailabilityRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewerAvailabilityNotFoundException(availabilityId));

            var exception = await Assert.ThrowsAsync<InterviewerAvailabilityNotFoundException>(
                () => _mediatorMock.Object.Send(new GetInterviewerAvailabilityRequest(availabilityId))
            );

            Assert.NotNull(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetInterviewerAvailabilityRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchInterviewerAvailabilities_ReturnsPagedResponse()
        {
            var request = new SearchInterviewerAvailabilitiesCommand
            {
                InterviewerId = Guid.NewGuid(),
                IsAvailable = true,
                PageNumber = 1,
                PageSize = 10
            };
            var response1 = new InterviewerAvailabilityResponse(Guid.NewGuid(), request.InterviewerId.Value, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1).AddHours(2), true);
            var response2 = new InterviewerAvailabilityResponse(Guid.NewGuid(), request.InterviewerId.Value, DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(2).AddHours(2), true);
            var pagedList = new PagedList<InterviewerAvailabilityResponse>(new[] { response1, response2 }, 1, 10, 2);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchInterviewerAvailabilitiesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchInterviewerAvailabilitiesCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewerAvailability_ReturnsUpdatedResponse()
        {
            var availabilityId = Guid.NewGuid();
            var request = new UpdateInterviewerAvailabilityCommand(
                availabilityId,
                InterviewerId: Guid.NewGuid(),
                StartTime: DateTime.UtcNow.AddDays(1),
                EndTime: DateTime.UtcNow.AddDays(1).AddHours(3),
                IsAvailable: false
            );
            var response = new UpdateInterviewerAvailabilityResponse(availabilityId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInterviewerAvailabilityCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(availabilityId, result.Id);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateInterviewerAvailabilityCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewerAvailability_ThrowsExceptionIfNotFound()
        {
            var availabilityId = Guid.NewGuid();
            var request = new UpdateInterviewerAvailabilityCommand(
                availabilityId,
                InterviewerId: Guid.NewGuid(),
                StartTime: DateTime.UtcNow.AddDays(1),
                EndTime: DateTime.UtcNow.AddDays(1).AddHours(3),
                IsAvailable: false
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInterviewerAvailabilityCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewerAvailabilityNotFoundException(availabilityId));

            var exception = await Assert.ThrowsAsync<InterviewerAvailabilityNotFoundException>(() => _mediatorMock.Object.Send(request));

            Assert.NotNull(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateInterviewerAvailabilityCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
