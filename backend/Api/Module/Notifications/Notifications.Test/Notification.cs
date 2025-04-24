using Moq;
using Xunit;
using TalentMesh.Module.Notifications.Application.Notifications.Create.v1;
using TalentMesh.Module.Notifications.Application.Notifications.Delete.v1;
using TalentMesh.Module.Notifications.Application.Notifications.Get.v1;
using TalentMesh.Module.Notifications.Application.Notifications.Search.v1;
using TalentMesh.Module.Notifications.Application.Notifications.Update.v1;
using TalentMesh.Module.Notifications.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Notifications.Domain;
using TalentMesh.Framework.Core.Caching;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Notifications.Tests
{
    public class NotificationHandlerTests
    {
        private readonly Mock<IRepository<Notification>> _repositoryMock;
        private readonly Mock<IReadRepository<Notification>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateNotificationHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteNotificationHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetNotificationHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchNotificationsHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateNotificationHandler>> _updateLoggerMock;

        private readonly CreateNotificationHandler _createHandler;
        private readonly DeleteNotificationHandler _deleteHandler;
        private readonly GetNotificationHandler _getHandler;
        private readonly SearchNotificationsHandler _searchHandler;
        private readonly UpdateNotificationHandler _updateHandler;

        public NotificationHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<Notification>>();
            _readRepositoryMock = new Mock<IReadRepository<Notification>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateNotificationHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteNotificationHandler>>();
            _getLoggerMock = new Mock<ILogger<GetNotificationHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchNotificationsHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateNotificationHandler>>();

            _createHandler = new CreateNotificationHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteNotificationHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetNotificationHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchNotificationsHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateNotificationHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateNotification_ReturnsNotificationResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var entity = "interview";
            var entityType = "interview";
            var message = "interview notification";
            var request = new CreateNotificationCommand(userId, entity, entityType, message);
            var expectedNotification = Notification.Create(request.UserId!, request.Entity!, request.EntityType, request.Message);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedNotification);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteNotification_DeletesSuccessfully()
        {
            // Arrange
            var existingNotification = Notification.Create(Guid.NewGuid(), "interview", "interview", "interview notification");
            var NotificationId = existingNotification.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(NotificationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingNotification);

            // Act
            await _deleteHandler.Handle(new DeleteNotificationCommand(NotificationId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingNotification, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(NotificationId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteNotification_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var NotificationId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(NotificationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Notification?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotificationNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteNotificationCommand(NotificationId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(NotificationId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetNotification_ReturnsNotificationResponse()
        {
            // Arrange
            var expectedNotification = Notification.Create(Guid.NewGuid(), "interview", "interview", "interview notification");
            var NotificationId = expectedNotification.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(NotificationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedNotification);

            _cacheServiceMock.Setup(cache => cache.GetAsync<NotificationResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((NotificationResponse?)null);

            // Act
            var result = await _getHandler.Handle(new GetNotificationRequest(NotificationId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedNotification.Id, result.Id);
            Assert.Equal(expectedNotification.Entity, result.Entity);
            Assert.Equal(expectedNotification.EntityType, result.EntityType);
            Assert.Equal(expectedNotification.Message, result.Message);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(NotificationId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<NotificationResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetNotification_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var NotificationId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(NotificationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Notification?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotificationNotFoundException>(() =>
                _getHandler.Handle(new GetNotificationRequest(NotificationId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(NotificationId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchNotifications_ReturnsPagedNotificationResponse()
        {
            // Arrange
            var request = new SearchNotificationsCommand
            {
                UserId = Guid.NewGuid(),
                Entity = "interview",
                EntityType = "interview",
                PageNumber = 1,
                PageSize = 10
            };

            var Notifications = new List<NotificationResponse>
            {
                new NotificationResponse(Guid.NewGuid(), Guid.NewGuid(), "interview", "interview", "interview notification"),
                new NotificationResponse(Guid.NewGuid(), Guid.NewGuid(), "profile", "profile", "profile notification")
            };
            var totalCount = Notifications.Count;

            // Mock returns List<Notification> (domain entities)
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchNotificationSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Notifications);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchNotificationSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.Entity == "interview" &&
                item.EntityType == "interview"
            );

            Assert.Contains(result.Items, item =>
                item.Entity == "profile" &&
                item.EntityType == "profile"
            );

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchNotificationSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchNotificationSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Fact]
        public async Task UpdateNotification_ReturnsUpdatedNotificationResponse()
        {
            // Arrange
            var existingNotification = Notification.Create(Guid.NewGuid(), "interview", "interview", "interview notification");
            var NotificationId = existingNotification.Id;
            var request = new UpdateNotificationCommand(
                NotificationId,
                Guid.NewGuid(),
                "interview",
                "interview",
                "interview notification"
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(NotificationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingNotification);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(NotificationId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(NotificationId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateNotification_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var NotificationId = Guid.NewGuid();
            var request = new UpdateNotificationCommand(NotificationId, Guid.NewGuid(), "interview", "interview", "interview notification");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(NotificationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Notification?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotificationNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(NotificationId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}
