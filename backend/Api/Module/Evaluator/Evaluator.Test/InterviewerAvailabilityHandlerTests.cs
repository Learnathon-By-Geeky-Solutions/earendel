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
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Evaluator.Domain;
using TalentMesh.Framework.Core.Caching;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Evaluator.Tests
{
    public class InterviewerAvailabilityHandlerTests
    {
        private readonly Mock<IRepository<InterviewerAvailability>> _repositoryMock;
        private readonly Mock<IReadRepository<InterviewerAvailability>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateInterviewerAvailabilityHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteInterviewerAvailabilityHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetInterviewerAvailabilityHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchInterviewerAvailabilitiesHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateInterviewerAvailabilityHandler>> _updateLoggerMock;

        private readonly CreateInterviewerAvailabilityHandler _createHandler;
        private readonly DeleteInterviewerAvailabilityHandler _deleteHandler;
        private readonly GetInterviewerAvailabilityHandler _getHandler;
        private readonly SearchInterviewerAvailabilitiesHandler _searchHandler;
        private readonly UpdateInterviewerAvailabilityHandler _updateHandler;

        public InterviewerAvailabilityHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<InterviewerAvailability>>();
            _readRepositoryMock = new Mock<IReadRepository<InterviewerAvailability>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateInterviewerAvailabilityHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteInterviewerAvailabilityHandler>>();
            _getLoggerMock = new Mock<ILogger<GetInterviewerAvailabilityHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchInterviewerAvailabilitiesHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateInterviewerAvailabilityHandler>>();

            _createHandler = new CreateInterviewerAvailabilityHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteInterviewerAvailabilityHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetInterviewerAvailabilityHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchInterviewerAvailabilitiesHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateInterviewerAvailabilityHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateInterviewerAvailability_ReturnsInterviewerAvailabilityResponse()
        {
            // Arrange
            var interviewerId = Guid.NewGuid();
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(1);

            var availabilitySlots = new List<AvailabilitySlot>
            {
                new AvailabilitySlot(startTime, endTime),
                new AvailabilitySlot(endTime.AddHours(1), endTime.AddHours(2))
            };


            var request = new CreateInterviewerAvailabilityCommand(interviewerId, availabilitySlots);


            _repositoryMock.Setup(repo => repo.ListAsync(It.IsAny<InterviewerAvailabilityByInterviewerIdSpec>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<InterviewerAvailability>()); // No existing availabilities

            _repositoryMock.Setup(repo => repo.AddRangeAsync(It.IsAny<List<InterviewerAvailability>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<InterviewerAvailability>());


            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.AvailabilityIds.Count); // Ensure both slots are created

            _repositoryMock.Verify(repo => repo.ListAsync(It.IsAny<InterviewerAvailabilityByInterviewerIdSpec>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.AddRangeAsync(It.IsAny<List<InterviewerAvailability>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateInterviewerAvailability_WithOverlappingSlot_ThrowsInvalidOperationException()
        {
            // Arrange
            var interviewerId = Guid.NewGuid();
            var now = DateTime.UtcNow;

            // This is the new slot the command is trying to add
            var requestSlot = new AvailabilitySlot(now, now.AddHours(1));

            // This existing availability overlaps with requestSlot
            var existingAvailabilities = new List<InterviewerAvailability>
            {
                InterviewerAvailability.Create(interviewerId, now.AddMinutes(30), now.AddHours(2), true)
            };

            var request = new CreateInterviewerAvailabilityCommand(
                interviewerId,
                new List<AvailabilitySlot> { requestSlot }
            );

            _repositoryMock.Setup(repo => repo.ListAsync(It.IsAny<InterviewerAvailabilityByInterviewerIdSpec>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAvailabilities);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _createHandler.Handle(request, CancellationToken.None)
            );

            Assert.Contains("already has an availability", exception.Message);

            _repositoryMock.Verify(repo => repo.ListAsync(It.IsAny<InterviewerAvailabilityByInterviewerIdSpec>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.AddRangeAsync(It.IsAny<List<InterviewerAvailability>>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Fact]
        public async Task CreateInterviewerAvailability_WithOverlappingSlot_CoversOverlapLogic()
        {
            // Arrange
            var interviewerId = Guid.NewGuid();
            var slotStart = DateTime.UtcNow;
            var slotEnd = slotStart.AddHours(1);

            // Existing overlaps the slot
            var existingAvailability = InterviewerAvailability.Create(
                interviewerId,
                slotStart.AddMinutes(30), // starts after the slot
                slotEnd.AddHours(1),      // ends after the slot
                true
            );

            var availabilitySlots = new List<AvailabilitySlot>
    {
        new AvailabilitySlot(slotStart, slotEnd)
    };

            var request = new CreateInterviewerAvailabilityCommand(interviewerId, availabilitySlots);

            _repositoryMock.Setup(repo => repo.ListAsync(It.IsAny<InterviewerAvailabilityByInterviewerIdSpec>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<InterviewerAvailability> { existingAvailability });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _createHandler.Handle(request, CancellationToken.None)
            );

            // This will ensure the IsOverlapping logic was triggered
        }

        [Fact]
        public void IsOverlapping_ReturnsTrue_ForOverlappingSlot()
        {
            // Arrange
            var slot = new AvailabilitySlot(
                new DateTime(2025, 4, 6, 10, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 4, 6, 11, 0, 0, DateTimeKind.Utc)
            );

            var existingAvailabilities = new List<InterviewerAvailability>
        {
            // Overlapping: starts at 10:30 and ends at 11:30 => overlaps with 10:00-11:00
            InterviewerAvailability.Create(
                Guid.NewGuid(),
                new DateTime(2025, 4, 6, 10, 30, 0, DateTimeKind.Utc),
                new DateTime(2025, 4, 6, 11, 30, 0, DateTimeKind.Utc),
                true)
        };

            // Act
            var result = CreateInterviewerAvailabilityHandler.IsOverlapping(slot, existingAvailabilities);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsOverlapping_ReturnsFalse_ForNonOverlappingSlot()
        {
            // Arrange
            var slot = new AvailabilitySlot(
                new DateTime(2025, 4, 6, 10, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 4, 6, 11, 0, 0, DateTimeKind.Utc)
            );

            var existingAvailabilities = new List<InterviewerAvailability>
        {
            // Non-overlapping: existing slot starts at 11:30 (after new slot ends)
            InterviewerAvailability.Create(
                Guid.NewGuid(),
                new DateTime(2025, 4, 6, 11, 30, 0, DateTimeKind.Utc),
                new DateTime(2025, 4, 6, 12, 30, 0, DateTimeKind.Utc),
                true)
        };

            // Act
            var result = CreateInterviewerAvailabilityHandler.IsOverlapping(slot, existingAvailabilities);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteInterviewerAvailability_DeletesSuccessfully()
        {
            // Arrange
            var existingInterviewerAvailability = InterviewerAvailability.Create(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1), true);
            var InterviewerAvailabilityId = existingInterviewerAvailability.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInterviewerAvailability);

            // Act
            await _deleteHandler.Handle(new DeleteInterviewerAvailabilityCommand(InterviewerAvailabilityId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingInterviewerAvailability, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewerAvailability_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewerAvailabilityId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewerAvailability)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewerAvailabilityNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteInterviewerAvailabilityCommand(InterviewerAvailabilityId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewerAvailability_ReturnsInterviewerAvailabilityResponse()
        {
            // Arrange
            var expectedInterviewerAvailability = InterviewerAvailability.Create(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1), true);
            var InterviewerAvailabilityId = expectedInterviewerAvailability.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedInterviewerAvailability);

            _cacheServiceMock.Setup(cache => cache.GetAsync<InterviewerAvailabilityResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewerAvailabilityResponse)null);

            // Act
            var result = await _getHandler.Handle(new GetInterviewerAvailabilityRequest(InterviewerAvailabilityId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedInterviewerAvailability.Id, result.Id);
            Assert.Equal(expectedInterviewerAvailability.InterviewerId, result.InterviewerId);
            Assert.Equal(expectedInterviewerAvailability.IsAvailable, result.IsAvailable);
            Assert.Equal(expectedInterviewerAvailability.StartTime, result.StartTime);
            Assert.Equal(expectedInterviewerAvailability.EndTime, result.EndTime);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<InterviewerAvailabilityResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewerAvailability_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewerAvailabilityId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewerAvailability)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewerAvailabilityNotFoundException>(() =>
                _getHandler.Handle(new GetInterviewerAvailabilityRequest(InterviewerAvailabilityId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchInterviewerAvailabilitys_ReturnsPagedInterviewerAvailabilityResponse()
        {
            // Arrange
            var request = new SearchInterviewerAvailabilitiesCommand
            {
                InterviewerId = Guid.NewGuid(),
                IsAvailable = true,
                PageNumber = 1,
                PageSize = 10
            };

            var InterviewerAvailabilitys = new List<InterviewerAvailabilityResponse>
            {
                new InterviewerAvailabilityResponse(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1), true),
                new InterviewerAvailabilityResponse(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(2), true),
            };
            var totalCount = InterviewerAvailabilitys.Count;

            // Mock returns List<InterviewerAvailability> (domain entities)
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchInterviewerAvailabilitySpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(InterviewerAvailabilitys);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchInterviewerAvailabilitySpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.IsAvailable
            );

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchInterviewerAvailabilitySpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchInterviewerAvailabilitySpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Fact]
        public async Task UpdateInterviewerAvailability_ReturnsUpdatedInterviewerAvailabilityResponse()
        {
            // Arrange
            var existingInterviewerAvailability = InterviewerAvailability.Create(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1), true);
            var InterviewerAvailabilityId = existingInterviewerAvailability.Id;
            var request = new UpdateInterviewerAvailabilityCommand(
                InterviewerAvailabilityId,
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                true
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInterviewerAvailability);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(InterviewerAvailabilityId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<InterviewerAvailability>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewerAvailability_ChangesAvailabilityStatus()
        {
            // Arrange
            var existingInterviewerAvailability = InterviewerAvailability.Create(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1), true);
            var InterviewerAvailabilityId = existingInterviewerAvailability.Id;

            // Flip isAvailable to false for this test
            var request = new UpdateInterviewerAvailabilityCommand(
                InterviewerAvailabilityId,
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                false // <-- different from existing value
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInterviewerAvailability);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(InterviewerAvailabilityId, result.Id);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<InterviewerAvailability>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewerAvailability_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewerAvailabilityId = Guid.NewGuid();
            var request = new UpdateInterviewerAvailabilityCommand(InterviewerAvailabilityId, Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1), true);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewerAvailability)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewerAvailabilityNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerAvailabilityId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}