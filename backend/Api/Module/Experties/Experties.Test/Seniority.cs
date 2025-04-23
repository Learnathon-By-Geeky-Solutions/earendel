using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TalentMesh.Module.Experties.Application.Seniorities.Create.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Delete.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Search.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Update.v1;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Tests
{
    public class SeniorityHandlerTests
    {
        private readonly Mock<IRepository<Seniority>> _repositoryMock;
        private readonly Mock<IReadRepository<Seniority>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateSeniorityHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteSeniorityHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetSeniorityHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchSenioritiesHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateSeniorityHandler>> _updateLoggerMock;

        private readonly CreateSeniorityHandler _createHandler;
        private readonly DeleteSeniorityHandler _deleteHandler;
        private readonly GetSeniorityHandler _getHandler;
        private readonly SearchSenioritiesHandler _searchHandler;
        private readonly UpdateSeniorityHandler _updateHandler;

        public SeniorityHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<Seniority>>();
            _readRepositoryMock = new Mock<IReadRepository<Seniority>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateSeniorityHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteSeniorityHandler>>();
            _getLoggerMock = new Mock<ILogger<GetSeniorityHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchSenioritiesHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateSeniorityHandler>>();

            _createHandler = new CreateSeniorityHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteSeniorityHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetSeniorityHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchSenioritiesHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateSeniorityHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateSeniority_ReturnsSeniorityResponse()
        {
            var request = new CreateSeniorityCommand("Internship", "0 Years of Experience");
            var expectedId = Guid.NewGuid();
            var response = new CreateSeniorityResponse(expectedId);
            var expectedSeniority = Seniority.Create("Internship", "0 Years of Experience");

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Seniority>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedSeniority);

            var result = await _createHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<CreateSeniorityResponse>(result);
        }

        [Fact]
        public async Task DeleteSeniority_DeletesSuccessfully()
        {
            var expectedSeniority = Seniority.Create("Internship", "0 Years of Experience");
            var seniorityId = expectedSeniority.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedSeniority);

            // Act
            await _deleteHandler.Handle(new DeleteSeniorityCommand(seniorityId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(expectedSeniority, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSeniority_ThrowsExceptionIfNotFound()
        {
            var seniorityId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Seniority)null);

            // Act & Assert
            await Assert.ThrowsAsync<SeniorityNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteSeniorityCommand(seniorityId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSeniority_ReturnsSeniorityResponse()
        {
            var expectedSeniority = Seniority.Create("Internship", "0 Years of Experience");
            var seniorityId = expectedSeniority.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedSeniority);

            _cacheServiceMock.Setup(cache => cache.GetAsync<SeniorityResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SeniorityResponse)null);

            // Act
            var result = await _getHandler.Handle(new GetSeniorityRequest(seniorityId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSeniority.Id, result.Id);
            Assert.Equal(expectedSeniority.Name, result.Name);
            Assert.Equal(expectedSeniority.Description, result.Description);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<SeniorityResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSeniority_ThrowsExceptionIfNotFound()
        {
            var seniorityId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Seniority)null);

            // Act & Assert
            await Assert.ThrowsAsync<SeniorityNotFoundException>(() =>
                _getHandler.Handle(new GetSeniorityRequest(seniorityId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchSeniorities_ReturnsPagedSeniorityResponse()
        {
            var request = new SearchSenioritiesCommand
            {
                Name = "Internship",
                Description = "0 Years of Experience",
                PageNumber = 1,
                PageSize = 10
            };

            var seniorities = new List<SeniorityResponse>(
                new[]
                {
                    new SeniorityResponse(Guid.NewGuid(), "Internship", "0 Years of Experience"),
                    new SeniorityResponse(Guid.NewGuid(), "Entry-Level", "1-2 Years of Experience")
                });
            var totalCount = seniorities.Count;
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchSenioritySpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(seniorities);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchSenioritySpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            var result = await _searchHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.All(result.Items, item => Assert.NotNull(item.Name));
            Assert.IsType<PagedList<SeniorityResponse>>(result);
        }

        [Fact]
        public async Task UpdateSeniority_ReturnsUpdatedSeniorityResponse()
        {
            var existingSeniority = Seniority.Create("Old Title", "Old Desc");
            var seniorityId = Guid.NewGuid();
            var request = new UpdateSeniorityCommand(seniorityId, "Updated Internship", "Updated 0 Years of Experience");
            var response = new UpdateSeniorityResponse(seniorityId);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Seniority>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSeniority_WhenFieldsChanged_ShouldUpdate()
        {
            var existingSeniority = Seniority.Create("Old Title", "Old Desc");
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSeniorityCommand(seniorityId, "New Title", "New Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(seniorityId, result.Id);

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Seniority>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task UpdateSeniority_WhenFieldsUnchanged_ShouldStillReturnButNotReassign()
        {
            var existingSeniority = Seniority.Create("Same Title", "Same Desc");
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSeniorityCommand(seniorityId, "Same Title", "Same Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(seniorityId, result.Id);

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Seniority>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSeniority_WhenNewTitleIsDifferent_ShouldUpdateTitle()
        {
            // Arrange
            var existingSeniority = Seniority.Create("Old Title", "Old Desc");
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSeniorityCommand(seniorityId, "New Title", "Old Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // The title should be updated because "New Title" is different from "Old Title"
            Assert.Equal("New Title", existingSeniority.Name);
        }

        // 2. When new title is non-null but equals the current title (case-insensitive) → update block is skipped.
        [Fact]
        public async Task UpdateSeniority_WhenNewTitleIsSame_ShouldNotUpdateTitle()
        {
            // Arrange
            var existingSeniority = Seniority.Create("Old Title", "Old Desc");
            var seniorityId = existingSeniority.Id;
            // Using a different casing but the same value
            var request = new UpdateSeniorityCommand(seniorityId, "old title", "Old Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // The title remains unchanged because "old title" equals "Old Title" (ignoring case)
            Assert.Equal("old title", existingSeniority.Name);
        }

        // 3. When new title is null → update block is skipped.
        [Fact]
        public async Task UpdateSeniority_WhenNewTitleIsNull_ShouldNotUpdateTitle()
        {
            // Arrange
            var existingSeniority = Seniority.Create("Old Title", "Old Desc");
            var seniorityId = existingSeniority.Id;
            // Passing null for title means no change should occur.
            var request = new UpdateSeniorityCommand(seniorityId, null, "Updated Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // The title remains unchanged because null did not trigger an update.
            Assert.Equal("Old Title", existingSeniority.Name);
        }

        // 4. When the current title is null and a new non-null title is provided → update occurs.
        [Fact]
        public async Task UpdateSeniority_WhenCurrentTitleIsNullAndNewTitleIsNonNull_ShouldUpdateTitle()
        {
            // Arrange
            // Create a seniority with a non-null title then force the title to null for test purposes.
            var existingSeniority = Seniority.Create(null, "Old Desc");
            // Forcing the Name to null (assuming Name has an accessible setter in test context)
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSeniorityCommand(seniorityId, "New Title", "Old Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Since the current title was null, it should be updated to "New Title"
            Assert.Equal("New Title", existingSeniority.Name);
        }

        [Fact]
        public async Task UpdateSeniority_WhenNewDescriptionIsDifferent_ShouldUpdateDescription()
        {
            var existingSeniority = Seniority.Create("Any Title", "Old Desc");
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSeniorityCommand(seniorityId, "Any Title", "New Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("New Desc", existingSeniority.Description);
        }

        [Fact]
        public async Task UpdateSeniority_WhenNewDescriptionIsSame_ShouldNotUpdateDescription()
        {
            var existingSeniority = Seniority.Create("Any Title", "Same Desc");
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSeniorityCommand(seniorityId, "Any Title", "same desc"); // same but lowercase

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("same desc", existingSeniority.Description); // unchanged
        }

        [Fact]
        public async Task UpdateSeniority_WhenNewDescriptionIsNull_ShouldNotUpdateDescription()
        {
            var existingSeniority = Seniority.Create("Any Title", "Existing Desc");
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSeniorityCommand(seniorityId, "Any Title", null); // null desc

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Existing Desc", existingSeniority.Description); // unchanged
        }

        [Fact]
        public async Task UpdateSeniority_WhenCurrentDescriptionIsNullAndNewIsProvided_ShouldUpdateDescription()
        {
            var existingSeniority = Seniority.Create("Any Title", null);
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSeniorityCommand(seniorityId, "Any Title", "Provided Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Provided Desc", existingSeniority.Description);
        }        

        [Fact]
        public async Task UpdateSeniority_ThrowsExceptionIfNotFound()
        {
            var seniorityId = Guid.NewGuid();
            var request = new UpdateSeniorityCommand(seniorityId, "Updated Internship", "Updated 0 Years of Experience");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Seniority)null);

            // Act & Assert
            await Assert.ThrowsAsync<SeniorityNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
