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