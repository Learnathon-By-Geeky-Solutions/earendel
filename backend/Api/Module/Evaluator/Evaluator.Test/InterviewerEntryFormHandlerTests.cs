using System;
using System.Threading;
using System.Threading.Tasks;
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
using TalentMesh.Module.Evaluator.Domain;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Evaluator.Tests
{
    public class InterviewerEntryFormHandlerTests
    {
        private readonly Mock<IRepository<InterviewerEntryForm>> _repositoryMock;
        private readonly Mock<IReadRepository<InterviewerEntryForm>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateInterviewerEntryFormHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteInterviewerEntryFormHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetInterviewerEntryFormHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchInterviewerEntryFormsHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateInterviewerEntryFormHandler>> _updateLoggerMock;

        private readonly CreateInterviewerEntryFormHandler _createHandler;
        private readonly DeleteInterviewerEntryFormHandler _deleteHandler;
        private readonly GetInterviewerEntryFormHandler _getHandler;
        private readonly SearchInterviewerEntryFormsHandler _searchHandler;
        private readonly UpdateInterviewerEntryFormHandler _updateHandler;

        public InterviewerEntryFormHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<InterviewerEntryForm>>();
            _readRepositoryMock = new Mock<IReadRepository<InterviewerEntryForm>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateInterviewerEntryFormHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteInterviewerEntryFormHandler>>();
            _getLoggerMock = new Mock<ILogger<GetInterviewerEntryFormHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchInterviewerEntryFormsHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateInterviewerEntryFormHandler>>();

            _createHandler = new CreateInterviewerEntryFormHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteInterviewerEntryFormHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetInterviewerEntryFormHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchInterviewerEntryFormsHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateInterviewerEntryFormHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateInterviewerEntryForm_ReturnsInterviewerEntryFormResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new CreateInterviewerEntryFormCommand(userId, "string");
            var expectedInterviewerEntryForm = InterviewerEntryForm.Create(request.UserId!, request.AdditionalInfo!);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<InterviewerEntryForm>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedInterviewerEntryForm);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<InterviewerEntryForm>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewerEntryForm_DeletesSuccessfully()
        {
            // Arrange
            var existingInterviewerEntryForm = InterviewerEntryForm.Create(Guid.NewGuid(), "string");
            var InterviewerEntryFormId = existingInterviewerEntryForm.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerEntryFormId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingInterviewerEntryForm);

            // Act
            await _deleteHandler.Handle(new DeleteInterviewerEntryFormCommand(InterviewerEntryFormId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingInterviewerEntryForm, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerEntryFormId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewerEntryForm_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewerEntryFormId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerEntryFormId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewerEntryForm)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewerEntryFormNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteInterviewerEntryFormCommand(InterviewerEntryFormId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerEntryFormId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewerEntryForm_ReturnsInterviewerEntryFormResponse()
        {
            // Arrange
            var expectedInterviewerEntryForm = InterviewerEntryForm.Create(Guid.NewGuid(), "string");
            var InterviewerEntryFormId = expectedInterviewerEntryForm.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerEntryFormId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedInterviewerEntryForm);

            _cacheServiceMock.Setup(cache => cache.GetAsync<InterviewerEntryFormResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewerEntryFormResponse)null);

            // Act
            var result = await _getHandler.Handle(new GetInterviewerEntryFormRequest(InterviewerEntryFormId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedInterviewerEntryForm.Id, result.Id);
            Assert.Equal(expectedInterviewerEntryForm.UserId, result.UserId);
            Assert.Equal(expectedInterviewerEntryForm.AdditionalInfo, result.AdditionalInfo);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerEntryFormId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<InterviewerEntryFormResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewerEntryForm_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewerEntryFormId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerEntryFormId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewerEntryForm)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewerEntryFormNotFoundException>(() =>
                _getHandler.Handle(new GetInterviewerEntryFormRequest(InterviewerEntryFormId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerEntryFormId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchInterviewerEntryForms_ReturnsPagedInterviewerEntryFormResponse()
        {
            // Arrange
            var request = new SearchInterviewerEntryFormsCommand
            {
                AdditionalInfo = "string",
                Status = "pending",
                PageSize = 10
            };

            var InterviewerEntryForms = new List<InterviewerEntryFormResponse>
            {
                new InterviewerEntryFormResponse(Guid.NewGuid(), Guid.NewGuid(), "string", "pending" ),
                new InterviewerEntryFormResponse(Guid.NewGuid(), Guid.NewGuid(), "string1", "approved" )
            };
            var totalCount = InterviewerEntryForms.Count;

            // Mock returns List<InterviewerEntryForm> (domain entities)
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchInterviewerEntryFormSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(InterviewerEntryForms);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchInterviewerEntryFormSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.AdditionalInfo == "string" &&
                item.Status == "pending"
            );

            Assert.Contains(result.Items, item =>
                item.AdditionalInfo == "string1" &&
                item.Status == "approved"
            );

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchInterviewerEntryFormSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchInterviewerEntryFormSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Fact]
        public async Task UpdateInterviewerEntryForm_UpdatesAdditionalInfoAndStatus_WhenDifferent()
        {
            // Arrange
            var initialAdditionalInfo = "initial comment";
            var initialStatus = "pending"; // assume default status from Create or set it explicitly if needed
            var updatedAdditionalInfo = "updated comment";
            var updatedStatus = "approved";

            // Create an InterviewerEntryForm with initial values.
            // Note: If your Create method doesn't set the Status, you might need to use reflection or a test-specific method to set it.
            var existingInterviewerEntryForm = InterviewerEntryForm.Create(Guid.NewGuid(), initialAdditionalInfo);

            var interviewerEntryFormId = existingInterviewerEntryForm.Id;

            var request = new UpdateInterviewerEntryFormCommand(
                interviewerEntryFormId,
                Guid.NewGuid(),       // JobId (if applicable)
                updatedAdditionalInfo,
                updatedStatus
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(interviewerEntryFormId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(existingInterviewerEntryForm);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(interviewerEntryFormId, result.Id);
            Assert.Equal(updatedAdditionalInfo, existingInterviewerEntryForm.AdditionalInfo);
            Assert.Equal(updatedStatus, existingInterviewerEntryForm.Status);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(interviewerEntryFormId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<InterviewerEntryForm>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Update_UpdatesAdditionalInfo()
        {
            // Arrange
            var interviewerEntryForm = InterviewerEntryForm.Create(
                Guid.NewGuid(),
                "Info"
            );

            var newInfo = "New Info";

            // Act
            interviewerEntryForm.Update(newInfo, null); // null status to isolate comment update

            // Assert
            Assert.Equal(newInfo, interviewerEntryForm.AdditionalInfo);
        }

        [Fact]
        public void Update_UpdatesStatusInfo()
        {
            // Arrange
            var interviewerEntryForm = InterviewerEntryForm.Create(
                Guid.NewGuid(),
                "Info"
            );

            var newStatus = "done";

            // Act
            interviewerEntryForm.Update(null, newStatus); 

            // Assert
            Assert.Equal(newStatus, interviewerEntryForm.Status);
        }
        [Fact]
        public async Task UpdateInterviewerEntryForm_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var InterviewerEntryFormId = Guid.NewGuid();
            var request = new UpdateInterviewerEntryFormCommand(InterviewerEntryFormId, Guid.NewGuid(), "string", "rejected");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(InterviewerEntryFormId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((InterviewerEntryForm)null);

            // Act & Assert
            await Assert.ThrowsAsync<InterviewerEntryFormNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(InterviewerEntryFormId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}
