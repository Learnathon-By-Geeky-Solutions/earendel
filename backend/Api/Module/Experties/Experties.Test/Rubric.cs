using Moq;
using TalentMesh.Module.Experties.Application.Rubrics.Create.v1;
using TalentMesh.Module.Experties.Application.Rubrics.Delete.v1;
using TalentMesh.Module.Experties.Application.Rubrics.Get.v1;
using TalentMesh.Module.Experties.Application.Rubrics.Search.v1;
using TalentMesh.Module.Experties.Application.Rubrics.Update.v1;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Framework.Core.Persistence;
using Microsoft.Extensions.Logging;
using TalentMesh.Framework.Core.Caching;

namespace TalentMesh.Module.Experties.Tests
{
    public class RubricHandlerTests
    {
        private readonly Mock<IRepository<Rubric>> _repositoryMock;
        private readonly Mock<IReadRepository<Rubric>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateRubricHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteRubricHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetRubricHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchRubricsHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateRubricHandler>> _updateLoggerMock;

        private readonly CreateRubricHandler _createHandler;
        private readonly DeleteRubricHandler _deleteHandler;
        private readonly GetRubricHandler _getHandler;
        private readonly SearchRubricsHandler _searchHandler;
        private readonly UpdateRubricHandler _updateHandler;

        public RubricHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<Rubric>>();
            _readRepositoryMock = new Mock<IReadRepository<Rubric>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateRubricHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteRubricHandler>>();
            _getLoggerMock = new Mock<ILogger<GetRubricHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchRubricsHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateRubricHandler>>();

            _createHandler = new CreateRubricHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteRubricHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetRubricHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchRubricsHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateRubricHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateRubric_ReturnsRubricResponse()
        {
            // Arrange
            var subSkillId = Guid.NewGuid();
            var seniorityLevelId = Guid.NewGuid();
            var request = new CreateRubricCommand(subSkillId, seniorityLevelId, "Effective C#", "C# advanced topics", 0.8m);
            var expectedRubric = Rubric.Create(request.Title!, request.RubricDescription!, subSkillId, seniorityLevelId, request.Weight);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Rubric>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedRubric);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Rubric>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRubric_DeletesSuccessfully()
        {
            // Arrange
            var existingRubric = Rubric.Create("Effective C#", "C# advanced topics", Guid.NewGuid(), Guid.NewGuid(), 0.8m);
            var rubricId = existingRubric.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);

            // Act
            await _deleteHandler.Handle(new DeleteRubricCommand(rubricId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingRubric, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRubric_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var rubricId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Rubric)null);

            // Act & Assert
            await Assert.ThrowsAsync<RubricNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteRubricCommand(rubricId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetRubric_ReturnsRubricResponse()
        {
            // Arrange
            var expectedRubric = Rubric.Create("Effective C#", "C# advanced topics", Guid.NewGuid(), Guid.NewGuid(), 0.8m);
            var rubricId = expectedRubric.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedRubric);

            _cacheServiceMock.Setup(cache => cache.GetAsync<RubricResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RubricResponse)null);

            // Act
            var result = await _getHandler.Handle(new GetRubricRequest(rubricId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRubric.Id, result.Id);
            Assert.Equal(expectedRubric.Title, result.Title);
            Assert.Equal(expectedRubric.RubricDescription, result.RubricDescription);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<RubricResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetRubric_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var rubricId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Rubric)null);

            // Act & Assert
            await Assert.ThrowsAsync<RubricNotFoundException>(() =>
                _getHandler.Handle(new GetRubricRequest(rubricId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchRubrics_ReturnsPagedRubricResponse()
        {
            // Arrange
            var request = new SearchRubricsCommand
            {
                Title = "Effective",
                PageNumber = 1,
                PageSize = 10
            };

            // Create domain entities (Rubric), not DTOs
            var rubric1 = Rubric.Create("Effective C#", "C# advanced topics", Guid.NewGuid(), Guid.NewGuid(), 0.8m);
            var rubric2 = Rubric.Create("Effective Java", "Java advanced topics", Guid.NewGuid(), Guid.NewGuid(), 0.9m);
            var rubrics = new List<RubricResponse>
            {
                new RubricResponse(Guid.NewGuid(), "Effective C#", "C# advanced topics", Guid.NewGuid(), Guid.NewGuid(), 0.8m),
                new RubricResponse(Guid.NewGuid(), "Effective Java", "Java advanced topics", Guid.NewGuid(), Guid.NewGuid(), 0.9m)
            };
            var totalCount = rubrics.Count;

            // Mock returns List<Rubric> (domain entities)
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchRubricSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rubrics);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchRubricSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.Title == "Effective C#" &&
                item.RubricDescription == "C# advanced topics"
            );

            Assert.Contains(result.Items, item =>
                item.Title == "Effective Java" &&
                item.RubricDescription == "Java advanced topics"
            );

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchRubricSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchRubricSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateRubric_ReturnsUpdatedRubricResponse()
        {
            // Arrange
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                Guid.NewGuid(),
                Guid.NewGuid(),
                1.0m,
                "Updated Title",
                "Updated Desc"
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rubricId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Rubric>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        // 1. New title is non-null and different → should update.
        [Fact]
        public async Task UpdateRubric_WhenTitleChanged_ShouldUpdateTitle()
        {
            // Arrange
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,               // No update for SubSkillId
                null,               // No update for SeniorityLevelId
                null,               // No update for Weight
                "New Title",        // New title provided
                null                // No update for description
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal("New Title", existingRubric.Title);
        }

        // 2. New title is non-null but equals current title (ignoring case) → no update.
        [Fact]
        public async Task UpdateRubric_WhenTitleIsSame_ShouldNotUpdateTitle()
        {
            // Arrange
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                null,
                null,
                "old title",  // same title in different case
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal("Old Title", existingRubric.Title);
        }

        // 3. New title is null → update block skipped.
        [Fact]
        public async Task UpdateRubric_WhenTitleIsNull_ShouldNotUpdateTitle()
        {
            // Arrange
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                null,
                null,
                null,  // title is null
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal("Old Title", existingRubric.Title);
        }

        [Fact]
        public async Task UpdateRubric_WhenCurrentTitleIsNull_ShouldNotUpdateTitle()
        {
            // Arrange
            var existingRubric = Rubric.Create(null, "Old Desc", Guid.NewGuid(), Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                null,
                null,
                "Provided Title",
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal("Provided Title", existingRubric.Title);
        }

        // =======================
        // RubricDescription update tests
        // =======================

        // 1. New description is non-null and different → should update.
        [Fact]
        public async Task UpdateRubric_WhenDescriptionChanged_ShouldUpdateDescription()
        {
            // Arrange
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                null,
                null,
                null,
                "New Desc"  // update description
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal("New Desc", existingRubric.RubricDescription);
        }

        // 2. New description is non-null but equals current description (ignoring case) → no update.
        [Fact]
        public async Task UpdateRubric_WhenDescriptionIsSame_ShouldNotUpdateDescription()
        {
            // Arrange
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                null,
                null,
                null,
                "old desc"  // same description in different case
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal("Old Desc", existingRubric.RubricDescription);
        }

        // 3. New description is null → update block skipped.
        [Fact]
        public async Task UpdateRubric_WhenDescriptionIsNull_ShouldNotUpdateDescription()
        {
            // Arrange
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                null,
                null,
                null,
                null  // description is null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal("Old Desc", existingRubric.RubricDescription);
        }

        [Fact]
        public async Task UpdateRubric_WhenCurrentDescriptionIsNull_ShouldNotUpdateDescription()
        {
            // Arrange
            var existingRubric = Rubric.Create("Old Title", null, Guid.NewGuid(), Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                null,
                null,
                null,
                "Provided Desc"
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal("Provided Desc", existingRubric.RubricDescription);
        }

        // =======================
        // SubSkillId update tests
        // =======================

        // 1. New subSkillId is non-null and different → should update.
        [Fact]
        public async Task UpdateRubric_WhenSubSkillIdChanged_ShouldUpdateSubSkillId()
        {
            // Arrange
            var oldSubSkillId = Guid.NewGuid();
            var newSubSkillId = Guid.NewGuid();
            var existingRubric = Rubric.Create("Old Title", "Old Desc", oldSubSkillId, Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                newSubSkillId,  // update subSkillId
                null,
                null,
                null,
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(newSubSkillId, existingRubric.SubSkillId);
        }

        // 2. New subSkillId is non-null but same as current → no update.
        [Fact]
        public async Task UpdateRubric_WhenSubSkillIdIsSame_ShouldNotUpdateSubSkillId()
        {
            // Arrange
            var subSkillId = Guid.NewGuid();
            var existingRubric = Rubric.Create("Old Title", "Old Desc", subSkillId, Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                subSkillId,  // same value
                null,
                null,
                null,
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(subSkillId, existingRubric.SubSkillId);
        }

        // 3. New subSkillId is null → update block skipped.
        [Fact]
        public async Task UpdateRubric_WhenSubSkillIdIsNull_ShouldNotUpdateSubSkillId()
        {
            // Arrange
            var existingSubSkillId = Guid.NewGuid();
            var existingRubric = Rubric.Create("Old Title", "Old Desc", existingSubSkillId, Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null, // subSkillId null
                null,
                null,
                null,
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(existingSubSkillId, existingRubric.SubSkillId);
        }

        [Fact]
        public async Task UpdateRubric_WhenProvidedSubSkillIdIsNull_ShouldNotUpdateSubSkillId()
        {
            // Arrange
            var existingSubSkillId = Guid.NewGuid();
            var existingRubric = Rubric.Create("Old Title", "Old Desc", null, Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                existingSubSkillId,
                null,
                null,
                null,
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(existingSubSkillId, existingRubric.SubSkillId);
        }

        // =======================
        // SeniorityLevelId update tests
        // =======================

        // 1. New seniorityLevelId is non-null and different → should update.
        [Fact]
        public async Task UpdateRubric_WhenSeniorityLevelIdChanged_ShouldUpdateSeniorityLevelId()
        {
            // Arrange
            var oldSeniorityLevelId = Guid.NewGuid();
            var newSeniorityLevelId = Guid.NewGuid();
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), oldSeniorityLevelId, 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                newSeniorityLevelId,  // update seniorityLevelId
                null,
                null,
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(newSeniorityLevelId, existingRubric.SeniorityLevelId);
        }

        // 2. New seniorityLevelId is non-null but same as current → no update.
        [Fact]
        public async Task UpdateRubric_WhenSeniorityLevelIdIsSame_ShouldNotUpdateSeniorityLevelId()
        {
            // Arrange
            var seniorityLevelId = Guid.NewGuid();
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), seniorityLevelId, 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                seniorityLevelId,  // same value
                null,
                null,
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(seniorityLevelId, existingRubric.SeniorityLevelId);
        }

        // 3. New seniorityLevelId is null → update block skipped.
        [Fact]
        public async Task UpdateRubric_WhenSeniorityLevelIdIsNull_ShouldNotUpdateSeniorityLevelId()
        {
            // Arrange
            var existingSeniorityLevelId = Guid.NewGuid();
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), existingSeniorityLevelId, 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                null, // seniorityLevelId null
                null,
                null,
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(existingSeniorityLevelId, existingRubric.SeniorityLevelId);
        }

        [Fact]
        public async Task UpdateRubric_WhenUpdatedSeniorityLevelIdIsNull_ShouldNotUpdateSeniorityLevelId()
        {
            // Arrange
            var existingSeniorityLevelId = Guid.NewGuid();
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), null, 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                existingSeniorityLevelId, 
                null,
                null,
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(existingSeniorityLevelId, existingRubric.SeniorityLevelId);
        }

        // =======================
        // Weight update tests
        // =======================

        // 1. New weight is non-null, > 0, and different → should update.
        [Fact]
        public async Task UpdateRubric_WhenWeightChanged_ShouldUpdateWeight()
        {
            // Arrange
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), Guid.NewGuid(), 0.5m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                null,
                1.0m, // update weight
                null,
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(1.0m, existingRubric.Weight);
        }

        // 2. New weight is non-null but same as current → no update.
        [Fact]
        public async Task UpdateRubric_WhenWeightIsSame_ShouldNotUpdateWeight()
        {
            // Arrange
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), Guid.NewGuid(), 1.0m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                null,
                1.0m, // same weight
                null,
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(1.0m, existingRubric.Weight);
        }

        // 3. New weight is null → update block skipped.
        [Fact]
        public async Task UpdateRubric_WhenWeightIsNull_ShouldNotUpdateWeight()
        {
            // Arrange
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), Guid.NewGuid(), 1.0m);
            var rubricId = existingRubric.Id;
            var request = new UpdateRubricCommand(
                rubricId,
                null,
                null,
                null,  // weight null
                null,
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(1.0m, existingRubric.Weight);
        }

        // 4. New weight is zero or negative → update block skipped.
        [Fact]
        public async Task UpdateRubric_WhenWeightIsZeroOrNegative_ShouldNotUpdateWeight()
        {
            // Arrange
            var existingRubric = Rubric.Create("Old Title", "Old Desc", Guid.NewGuid(), Guid.NewGuid(), 1.0m);
            var rubricId = existingRubric.Id;
            // Test with zero
            var requestZero = new UpdateRubricCommand(
                rubricId,
                null,
                null,
                0m,
                null,
                null
            );
            // Test with negative weight
            var requestNegative = new UpdateRubricCommand(
                rubricId,
                null,
                null,
                -0.5m,
                null,
                null
            );
            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRubric);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingRubric, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _updateHandler.Handle(requestZero, CancellationToken.None);
            await _updateHandler.Handle(requestNegative, CancellationToken.None);

            // Assert: weight should remain unchanged (1.0m)
            Assert.Equal(1.0m, existingRubric.Weight);
        }

       [Fact]
        public async Task UpdateRubric_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var rubricId = Guid.NewGuid();
            var request = new UpdateRubricCommand(rubricId, Guid.NewGuid(), Guid.NewGuid(), 1.0m, "Title", "Desc");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Rubric)null);

            // Act & Assert
            await Assert.ThrowsAsync<RubricNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(rubricId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}