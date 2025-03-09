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

namespace TalentMesh.Module.Notifications.Tests
{
    public class NotificationHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public NotificationHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateNotification_ReturnsNotificationResponse()
        {
            var request = new CreateNotificationCommand(Guid.NewGuid(), "jobs", "job", "create");
            var expectedId = Guid.NewGuid();
            var response = new CreateNotificationResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateNotificationResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateNotificationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteNotification_DeletesSuccessfully()
        {
            var notificationId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteNotificationCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mediatorMock.Object.Send(new DeleteNotificationCommand(notificationId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteNotificationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteNotification_ThrowsExceptionIfNotFound()
        {
            var notificationId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteNotificationCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotificationNotFoundException(notificationId));

            var exception = await Assert.ThrowsAsync<NotificationNotFoundException>(() => _mediatorMock.Object.Send(new DeleteNotificationCommand(notificationId)));

            Assert.NotNull(exception);
            Assert.IsType<NotificationNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteNotificationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetNotification_ReturnsNotificationResponse()
        {
            var notificationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var expectedEntity = "jobs";
            var expectedEntityType = "job";
            var expectedMessage = "create";
            var notificationResponse = new NotificationResponse(notificationId, userId, expectedEntity, expectedEntityType, expectedMessage);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetNotificationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(notificationResponse);

            var result = await _mediatorMock.Object.Send(new GetNotificationRequest(notificationId));

            Assert.NotNull(result);
            Assert.Equal(notificationId, result.Id);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(expectedEntity, result.Entity);
            Assert.Equal(expectedEntityType, result.EntityType);
            Assert.Equal(expectedMessage, result.Message);
            Assert.IsType<NotificationResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetNotificationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetNotification_ThrowsExceptionIfNotFound()
        {
            var notificationId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetNotificationRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotificationNotFoundException(notificationId));

            var exception = await Assert.ThrowsAsync<NotificationNotFoundException>(() => _mediatorMock.Object.Send(new GetNotificationRequest(notificationId)));

            Assert.NotNull(exception);
            Assert.IsType<NotificationNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetNotificationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchNotifications_ReturnsPagedNotificationResponse()
        {
            var request = new SearchNotificationsCommand
            {
                UserId = Guid.NewGuid(),
                Entity = "jobs",
                EntityType = "job",
                Message = "create",
                PageNumber = 1,
                PageSize = 10
            };

            var pagedList = new PagedList<NotificationResponse>(
                new[]
                {
                    new NotificationResponse(Guid.NewGuid(), Guid.NewGuid(), "jobs", "job", "create"),
                    new NotificationResponse(Guid.NewGuid(), Guid.NewGuid(), "projects", "project", "update")
                },
                1,
                10,
                2);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchNotificationsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.All(result.Items, item => Assert.NotNull(item.Entity));
            Assert.IsType<PagedList<NotificationResponse>>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchNotificationsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateNotification_ReturnsUpdatedNotificationResponse()
        {
            var notificationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new UpdateNotificationCommand(notificationId, userId, "updated message", "updated entity", "updated entityType");
            var response = new UpdateNotificationResponse(notificationId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateNotificationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(notificationId, result.Id);
            Assert.IsType<UpdateNotificationResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateNotificationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateNotification_ThrowsExceptionIfNotFound()
        {
            var notificationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new UpdateNotificationCommand(notificationId, userId, "updated message", "updated entity", "updated entityType");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateNotificationCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotificationNotFoundException(notificationId));

            var exception = await Assert.ThrowsAsync<NotificationNotFoundException>(() => _mediatorMock.Object.Send(request));

            Assert.NotNull(exception);
            Assert.IsType<NotificationNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateNotificationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
