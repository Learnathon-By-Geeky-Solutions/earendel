using Moq;
using TalentMesh.Module.Experties.Application.SubSkills.Create.v1;
using TalentMesh.Module.Experties.Application.SubSkills.Delete.v1;
using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using TalentMesh.Module.Experties.Application.SubSkills.Search.v1;
using TalentMesh.Module.Experties.Application.SubSkills.Update.v1;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Framework.Core.Caching;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Tests
{

    public class SubSkillHandlerTests
    {
        private readonly Mock<IRepository<SubSkill>> _repositoryMock;
        private readonly Mock<IReadRepository<SubSkill>> _readRepositoryMock; // Declare Read Repository Mock
        private readonly Mock<ICacheService> _cacheServiceMock; // Add Cache Service Mock
        private readonly Mock<ILogger<CreateSubSkillHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteSubSkillHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetSubSkillHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchSubSkillsHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateSubSkillHandler>> _updateLoggerMock;

        private readonly CreateSubSkillHandler _createHandler;
        private readonly DeleteSubSkillHandler _deleteHandler;
        private readonly GetSubSkillHandler _getHandler;
        private readonly SearchSubSkillsHandler _searchHandler;
        private readonly UpdateSubSkillHandler _updateHandler;

        public SubSkillHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<SubSkill>>();
            _createLoggerMock = new Mock<ILogger<CreateSubSkillHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteSubSkillHandler>>();
            _getLoggerMock = new Mock<ILogger<GetSubSkillHandler>>();
            _cacheServiceMock = new Mock<ICacheService>(); // Initialize cache service mock
            _searchLoggerMock = new Mock<ILogger<SearchSubSkillsHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateSubSkillHandler>>();
            _readRepositoryMock = new Mock<IReadRepository<SubSkill>>(); // Initialize Read Repository Mock

            _createHandler = new CreateSubSkillHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteSubSkillHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetSubSkillHandler(_readRepositoryMock.Object, _cacheServiceMock.Object); // Correct parameters
            _searchHandler = new SearchSubSkillsHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateSubSkillHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateSubSkill_ReturnsSubSkillResponse()
        {
            // Arrange
            var skillId = Guid.NewGuid();
            var request = new CreateSubSkillCommand(skillId, "ASP.NET Core", "Web framework");

            // Use a fixed GUID for the expected subskill ID

            // Initialize expectedSubSkill using the expectedId
            var expectedSubSkill = SubSkill.Create(request.Name!, request.Description, request.SkillId);  // Pass the expected Id to Create

            _repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<SubSkill>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedSubSkill);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<SubSkill>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task DeleteSubSkill_DeletesSuccessfully()
        {
            // Arrange
            var existingSubSkill = SubSkill.Create("ASP.NET Core", "Web framework", Guid.NewGuid());
            var subSkillId = existingSubSkill.Id;

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(subSkillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSubSkill);

            _repositoryMock
                .Setup(repo => repo.DeleteAsync(existingSubSkill, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _deleteHandler.Handle(new DeleteSubSkillCommand(subSkillId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<SubSkill>(), It.IsAny<CancellationToken>()), Times.Once);
        }



        [Fact]
        public async Task DeleteSubSkill_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var subSkillId = Guid.NewGuid();

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(subSkillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((SubSkill)null); // Simulate sub-skill not found

            // Act & Assert
            await Assert.ThrowsAsync<SubSkillNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteSubSkillCommand(subSkillId), CancellationToken.None));

            // Verify that GetByIdAsync was called once
            _repositoryMock.Verify(repo => repo.GetByIdAsync(subSkillId, It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task GetSubSkill_ReturnsSubSkillResponse()
        {
            // Arrange
            var skillId = Guid.NewGuid();
            var subSkill = SubSkill.Create("ASP.NET Core", "Web framework", skillId);
            var subSkillId = subSkill.Id;

            // Mock repository to return the expected subSkill
            _readRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.Is<Guid>(id => id == subSkillId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(subSkill);

            // Mock cache service to simulate a cache miss
            _cacheServiceMock
                .Setup(cache => cache.GetAsync<SubSkillResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SubSkillResponse)null);

            // Mock cache service to store the retrieved subSkill
            _cacheServiceMock
                .Setup(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<SubSkillResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _getHandler.Handle(new GetSubSkillRequest(subSkillId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(subSkillId, result.Id);
            Assert.Equal(subSkill.Name, result.Name);
            Assert.Equal(subSkill.Description, result.Description);

            // Verify repository access
            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(subSkillId, It.IsAny<CancellationToken>()), Times.Once);

            // Verify cache operations
            _cacheServiceMock.Verify(cache => cache.GetAsync<SubSkillResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<SubSkillResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task GetSubSkill_ThrowsExceptionIfNotFound()
        {
            var subSkillId = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetByIdAsync(subSkillId, It.IsAny<CancellationToken>())).ReturnsAsync((SubSkill)null);

            await Assert.ThrowsAsync<SubSkillNotFoundException>(() => _getHandler.Handle(new GetSubSkillRequest(subSkillId), CancellationToken.None));
        }


        [Fact]
        public async Task SearchSubSkills_ReturnsPagedSubSkillResponse()
        {
            // Arrange
            var request = new SearchSubSkillsCommand
            {
                Name = "ASP.NET",
                PageNumber = 1,
                PageSize = 10
            };

            var subSkills = new List<SubSkillResponse> // Ensure this is List<SubSkillResponse>
    {
        new SubSkillResponse(Guid.NewGuid(), "ASP.NET Core", "Web framework", Guid.NewGuid()),
        new SubSkillResponse(Guid.NewGuid(), "Blazor", "Web UI framework for .NET", Guid.NewGuid())
    };

            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchSubSkillSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(subSkills); // Returning List<SubSkillResponse>, which matches repository method signature

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchSubSkillSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(subSkills.Count);

            // Act: Call the handler.
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert:
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            // Verify the returned SubSkillResponse items match the expected sub-skills.
            Assert.Contains(result.Items, item => item.Name == "ASP.NET Core" && item.Description == "Web framework");
            Assert.Contains(result.Items, item => item.Name == "Blazor" && item.Description == "Web UI framework for .NET");

            // Verify that repository methods were called exactly once.
            _readRepositoryMock.Verify(repo => repo.ListAsync(It.IsAny<SearchSubSkillSpecs>(), It.IsAny<CancellationToken>()), Times.Once);
            _readRepositoryMock.Verify(repo => repo.CountAsync(It.IsAny<SearchSubSkillSpecs>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task UpdateSubSkill_ReturnsUpdatedSubSkillResponse()
        {
            // Arrange
            var existingSubSkill = SubSkill.Create("Old ASP.NET Core", "Old description", Guid.NewGuid());
            var subSkillId = existingSubSkill.Id;
            var skillId = existingSubSkill.SkillId.GetValueOrDefault();
            var request = new UpdateSubSkillCommand(subSkillId, skillId, "Updated ASP.NET Core", "Updated description");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(subSkillId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(existingSubSkill);

            _repositoryMock.Setup(repo => repo.UpdateAsync(existingSubSkill, It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(subSkillId, result.Id);

            // Verify repository interactions
            _repositoryMock.Verify(repo => repo.GetByIdAsync(subSkillId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(existingSubSkill, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSubSkill_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var subSkillId = Guid.NewGuid();
            var skillId = Guid.NewGuid();
            var request = new UpdateSubSkillCommand(subSkillId, skillId, "Updated ASP.NET Core", "Updated description");

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(subSkillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((SubSkill)null); // Simulate skill not found

            // Act & Assert
            await Assert.ThrowsAsync<SubSkillNotFoundException>(() => _updateHandler.Handle(request, CancellationToken.None));

            // Verify that GetByIdAsync was called once
            _repositoryMock.Verify(repo => repo.GetByIdAsync(subSkillId, It.IsAny<CancellationToken>()), Times.Once);
        }


    }
}
