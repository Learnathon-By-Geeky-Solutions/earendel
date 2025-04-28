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
using TalentMesh.Module.Interviews.Domain;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Interviews.Application.Services;
using TalentMesh.Framework.Infrastructure.Messaging;

namespace TalentMesh.Module.Interviews.Tests
{
    public class InterviewHandlerTests
    {
        private readonly Mock<IRepository<Interview>> _repositoryMock;
        private readonly Mock<IReadRepository<Interview>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IMessageBus> _messageServiceMock;
        private readonly Mock<ILogger<CreateInterviewHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteInterviewHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetInterviewHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchInterviewsHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateInterviewHandler>> _updateLoggerMock;
        private readonly Mock<IZoomService> _zoomServiceMock;

        private readonly CreateInterviewHandler _createHandler;
        private readonly DeleteInterviewHandler _deleteHandler;
        private readonly GetInterviewHandler _getHandler;
        private readonly SearchInterviewsHandler _searchHandler;
        private readonly UpdateInterviewHandler _updateHandler;

        public InterviewHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<Interview>>();
            _readRepositoryMock = new Mock<IReadRepository<Interview>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _messageServiceMock = new Mock<IMessageBus>();
            _createLoggerMock = new Mock<ILogger<CreateInterviewHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteInterviewHandler>>();
            _getLoggerMock = new Mock<ILogger<GetInterviewHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchInterviewsHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateInterviewHandler>>();

            _zoomServiceMock = new Mock<IZoomService>();

            _createHandler = new CreateInterviewHandler(_createLoggerMock.Object, _repositoryMock.Object, _zoomServiceMock.Object, _messageServiceMock.Object);
            _deleteHandler = new DeleteInterviewHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetInterviewHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchInterviewsHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateInterviewHandler(_updateLoggerMock.Object, _repositoryMock.Object, _messageServiceMock.Object);

        }

        [Fact]
        public async Task CreateInterview_ReturnsInterviewResponse()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var interviewerId = Guid.NewGuid();
            var intervieweDate = DateTime.UtcNow;
            var status = "Pending";
            var notes = "Interview Note";
            var meetingId = "123456";

            var request = new CreateInterviewCommand(applicationId, interviewerId, Guid.NewGuid(), Guid.NewGuid(), intervieweDate, status, notes, meetingId);
            var expectedInterview = Interview.Create(request.ApplicationId!, request.InterviewerId!, request.CandidateId!, request.JobId!, request.InterviewDate, request.Status, request.Notes, request.MeetingId);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Interview>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedInterview);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Interview>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task DeleteInterview_DeletesSuccessfully()
        {
            // Arrange
            var existingInterview = Interview.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Pending", "Interview Notes", "123456");
            var InterviewId = existingInterview.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInterview);

            // Act
            await _deleteHandler.Handle(new DeleteInterviewCommand(InterviewId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingInterview, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterview_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Interview?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteInterviewCommand(InterviewId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterview_ReturnsInterviewResponse()
        {
            // Arrange
            var expectedInterview = Interview.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Pending", "Interview Notes", "123456");
            var InterviewId = expectedInterview.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(InterviewId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedInterview);

            _cacheServiceMock.Setup(cache => cache.GetAsync<InterviewResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewResponse?)null);

            // Act
            var result = await _getHandler.Handle(new GetInterviewRequest(InterviewId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedInterview.Id, result.Id);
            Assert.Equal(expectedInterview.ApplicationId, result.ApplicationId);
            Assert.Equal(expectedInterview.InterviewerId, result.InterviewerId);
            Assert.Equal(expectedInterview.InterviewDate, result.InterviewDate);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(InterviewId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<InterviewResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterview_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(InterviewId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Interview?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewNotFoundException>(() =>
                _getHandler.Handle(new GetInterviewRequest(InterviewId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(InterviewId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchInterviews_ReturnsPagedInterviewResponse()
        {
            // Arrange
            var request = new SearchInterviewsCommand
            {
                ApplicationId = Guid.NewGuid(),
                InterviewerId = Guid.NewGuid(),
                PageNumber = 1,
                PageSize = 10
            };

            var Interviews = new List<InterviewResponse>
            {
                new InterviewResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Pending", "Notes", "123456"),
                new InterviewResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Done", "Notes", "1234567")
            };
            var totalCount = Interviews.Count;

            // Mock returns List<Interview> (domain entities)
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchInterviewSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Interviews);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchInterviewSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.Status == "Pending" &&
                item.MeetingId == "123456"
            );

            Assert.Contains(result.Items, item =>
                item.Status == "Done" &&
                item.MeetingId == "1234567"
            );

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchInterviewSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchInterviewSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Fact]
        public async Task UpdateInterview_ReturnsUpdatedInterviewResponse()
        {
            // Arrange
            var existingInterview = Interview.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Pending", "Notes", "12345");
            var InterviewId = existingInterview.Id;
            var request = new UpdateInterviewCommand(
                InterviewId,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                "Pending",
                "Notes",
                "1234"
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInterview);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(InterviewId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Interview>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterview_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewId = Guid.NewGuid();
            var request = new UpdateInterviewCommand(InterviewId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Pending", "Notes", "12345");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Interview?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

