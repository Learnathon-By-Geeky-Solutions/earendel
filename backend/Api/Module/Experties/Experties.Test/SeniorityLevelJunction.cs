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
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Create.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Delete.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Update.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Search.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;

namespace TalentMesh.Module.Experties.Tests
{
    public class SeniorityLevelJunctionHandlerTests
    {
        private readonly Mock<IRepository<SeniorityLevelJunction>> _repositoryMock;
        private readonly Mock<IReadRepository<SeniorityLevelJunction>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateSeniorityLevelJunctionHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteSeniorityLevelJunctionHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetSeniorityLevelJunctionHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchSeniorityLevelJunctionHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateSeniorityLevelJunctionHandler>> _updateLoggerMock;

        private readonly CreateSeniorityLevelJunctionHandler _createHandler;
        private readonly DeleteSeniorityLevelJunctionHandler _deleteHandler;
        private readonly GetSeniorityLevelJunctionHandler _getHandler;
        private readonly SearchSeniorityLevelJunctionHandler _searchHandler;
        private readonly UpdateSeniorityLevelJunctionHandler _updateHandler;

        public SeniorityLevelJunctionHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<SeniorityLevelJunction>>();
            _readRepositoryMock = new Mock<IReadRepository<SeniorityLevelJunction>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateSeniorityLevelJunctionHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteSeniorityLevelJunctionHandler>>();
            _getLoggerMock = new Mock<ILogger<GetSeniorityLevelJunctionHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchSeniorityLevelJunctionHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateSeniorityLevelJunctionHandler>>();

            _createHandler = new CreateSeniorityLevelJunctionHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteSeniorityLevelJunctionHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetSeniorityLevelJunctionHandler(_readRepositoryMock.Object, _cacheServiceMock.Object, _getLoggerMock.Object);
            _searchHandler = new SearchSeniorityLevelJunctionHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateSeniorityLevelJunctionHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateSeniorityLevelJunction_ReturnsRubricResponse()
        {
            // Arrange
            var skillId = Guid.NewGuid();
            var seniorityLevelId = Guid.NewGuid();
            var request = new CreateSeniorityLevelJunctionCommand(skillId, new List<Guid> { seniorityLevelId });

            var expectedSeniority = SeniorityLevelJunction.Create(seniorityLevelId, skillId);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<SeniorityLevelJunction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedSeniority);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<SeniorityLevelJunction>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSeniorityLevelJunction_DeletesSuccessfully()
        {
            // Arrange
            var existingSeniority = SeniorityLevelJunction.Create(Guid.NewGuid(), Guid.NewGuid());
            var seniorityId = existingSeniority.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);

            // Act
            await _deleteHandler.Handle(new DeleteSeniorityLevelJunctionCommand(seniorityId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingSeniority, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSeniorityLevelJunction_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var seniorityId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((SeniorityLevelJunction)null);

            // Act & Assert
            await Assert.ThrowsAsync<SeniorityLevelJunctionNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteSeniorityLevelJunctionCommand(seniorityId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSeniorityLevelJunction_ReturnsSeniorityLevelJunctionResponse()
        {
            // Arrange
            var dummySeniority = Seniority.Create("Test Seniority", "Test Description");


            var expectedSeniority = SeniorityLevelJunction.Create(Guid.NewGuid(), Guid.NewGuid());
            // Ensure Seniority is set via reflection
            typeof(SeniorityLevelJunction)
                .GetProperty("Seniority", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(expectedSeniority, dummySeniority);

            var seniorityId = expectedSeniority.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedSeniority);

            _cacheServiceMock.Setup(cache => cache.GetAsync<SeniorityLevelJunctionResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SeniorityLevelJunctionResponse)null);

            // Act
            var result = await _getHandler.Handle(new GetSeniorityLevelJunctionRequest(seniorityId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSeniority.SkillId, result.SkillId);
            Assert.Equal(expectedSeniority.Id, result.Id);
            Assert.Equal(expectedSeniority.SeniorityLevelId, result.SeniorityLevelId);
            // Additionally, verify that Seniority is mapped correctly.
            Assert.NotNull(result.Seniority);
            Assert.Equal(dummySeniority.Id, result.Seniority.Id);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<SeniorityLevelJunctionResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task GetSeniorityLevelJunction_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var seniorityId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((SeniorityLevelJunction)null);

            // Act & Assert
            await Assert.ThrowsAsync<SeniorityLevelJunctionNotFoundException>(() =>
                _getHandler.Handle(new GetSeniorityLevelJunctionRequest(seniorityId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchSeniorityLevelJunctions_ReturnsPagedSeniorityLevelJunctionResponse()
        {
            // Arrange
            var seniorityLevelId = Guid.NewGuid();
            var skillId = Guid.NewGuid();

            var request = new SearchSeniorityLevelJunctionCommand
            {
                SeniorityLevelId = seniorityLevelId,
                SkillId = skillId,
                PageNumber = 1,
                PageSize = 10
            };

            var seniorities = new List<SeniorityLevelJunctionResponse>
            {
                new SeniorityLevelJunctionResponse(
                    Guid.NewGuid(),
                    seniorityLevelId,
                    skillId,
                    new SeniorityResponse(seniorityLevelId, "Junior", "Entry-level position")
                ),
                new SeniorityLevelJunctionResponse(
                    Guid.NewGuid(),
                    seniorityLevelId,
                    skillId,
                new SeniorityResponse(seniorityLevelId, "Senior", "High-level expertise")
                )
           };

            var totalCount = seniorities.Count;

            // Mock repository to return filtered results
            _readRepositoryMock
            .Setup(repo => repo.ListAsync(
                It.Is<SearchSeniorityLevelJunctionSpecs>(spec =>
                spec.GetType() == typeof(SearchSeniorityLevelJunctionSpecs)
                ),
                It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(seniorities);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(
                    It.Is<SearchSeniorityLevelJunctionSpecs>(spec =>
                        spec.GetType() == typeof(SearchSeniorityLevelJunctionSpecs)
                    ),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(totalCount);


            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.All(result.Items, item => Assert.Equal(seniorityLevelId, item.SeniorityLevelId));

        }
    }
}