using Moq;
using Xunit;
using TalentMesh.Module.Experties.Application.Skills.Create.v1;
using TalentMesh.Module.Experties.Application.Skills.Delete.v1;
using TalentMesh.Module.Experties.Application.Skills.Get.v1;
using TalentMesh.Module.Experties.Application.Skills.Search.v1;
using TalentMesh.Module.Experties.Application.Skills.Update.v1;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Framework.Core.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Framework.Core.Caching;

namespace TalentMesh.Module.Experties.Tests
{
    public class SkillHandlerTests
    {
        private readonly Mock<IRepository<Skill>> _repositoryMock;
        private readonly Mock<IReadRepository<Skill>> _readRepositoryMock; // Declare Read Repository Mock
        private readonly Mock<ICacheService> _cacheServiceMock; // Add Cache Service Mock
        private readonly Mock<ILogger<CreateSkillHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteSkillHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetSkillHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchSkillsHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateSkillHandler>> _updateLoggerMock;

        private readonly CreateSkillHandler _createHandler;
        private readonly DeleteSkillHandler _deleteHandler;
        private readonly GetSkillHandler _getHandler;
        private readonly SearchSkillsHandler _searchHandler;
        private readonly UpdateSkillHandler _updateHandler;

        public SkillHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<Skill>>();
            _createLoggerMock = new Mock<ILogger<CreateSkillHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteSkillHandler>>();
            _getLoggerMock = new Mock<ILogger<GetSkillHandler>>();
            _cacheServiceMock = new Mock<ICacheService>(); // Initialize cache service mock
            _searchLoggerMock = new Mock<ILogger<SearchSkillsHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateSkillHandler>>();
            _readRepositoryMock = new Mock<IReadRepository<Skill>>(); // Initialize Read Repository Mock

            _createHandler = new CreateSkillHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteSkillHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetSkillHandler(_readRepositoryMock.Object, _cacheServiceMock.Object); // Correct parameters
            _searchHandler = new SearchSkillsHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateSkillHandler(_updateLoggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task CreateSkill_ReturnsSkillResponse()
        {
            // Arrange
            var request = new CreateSkillCommand("C# Development", "Advanced C# programming");

            // Use a fixed GUID for the expected skill ID
            var expectedId = Guid.NewGuid();

            // Initialize expectedSkill using the expectedId
            var expectedSkill = Skill.Create(request.Name!, request.Description);  // Pass the expected Id to Create

            _repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Skill>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedSkill);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Skill>(), It.IsAny<CancellationToken>()), Times.Once);
        }



        [Fact]
        public async Task DeleteSkill_DeletesSuccessfully()
        {
            var existingSkill = Skill.Create("C#", "Advanced C#");
            var skillId = existingSkill.Id;
            _repositoryMock.Setup(repo => repo.GetByIdAsync(skillId, It.IsAny<CancellationToken>())).ReturnsAsync(existingSkill);
            _repositoryMock.Setup(repo => repo.DeleteAsync(existingSkill, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _deleteHandler.Handle(new DeleteSkillCommand(skillId), CancellationToken.None);

            _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Skill>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSkill_ThrowsExceptionIfNotFound()
        {
            var skillId = Guid.NewGuid();

            // Use GetByIdAsync instead of FindByIdAsync
            _repositoryMock.Setup(repo => repo.GetByIdAsync(skillId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Skill)null); // Simulate skill not found

            await Assert.ThrowsAsync<SkillNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteSkillCommand(skillId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(skillId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSkill_ReturnsSkillResponse()
        {
            // Arrange
            var skill = Skill.Create("C# Development", "Advanced C#");
            var skillId = skill.Id; // Use the skill's own ID

            // Setup the repository to return the skill when queried with the same ID
            _readRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.Is<Guid>(id => id == skillId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(skill);

            // Setup the cache to always return null (simulate a cache miss) regardless of the key
            _cacheServiceMock
                .Setup(cache => cache.GetAsync<SkillResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SkillResponse)null);

            // Setup the cache's SetAsync to simply return a completed task
            _cacheServiceMock
                .Setup(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<SkillResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _getHandler.Handle(new GetSkillRequest(skillId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(skillId, result.Id);
            Assert.Equal(skill.Name, result.Name);
            Assert.Equal(skill.Description, result.Description);

            // Verify that the repository's GetByIdAsync was called exactly once with the correct id.
            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(skillId, It.IsAny<CancellationToken>()), Times.Once);

            // Verify that the cache GetAsync and SetAsync were each called exactly once (ignoring the key)
            _cacheServiceMock.Verify(cache => cache.GetAsync<SkillResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<SkillResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSkill_ThrowsExceptionIfNotFound()
        {
            var skillId = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetByIdAsync(skillId, It.IsAny<CancellationToken>())).ReturnsAsync((Skill)null);

            await Assert.ThrowsAsync<SkillNotFoundException>(() => _getHandler.Handle(new GetSkillRequest(skillId), CancellationToken.None));
        }

        [Fact]
        public async Task SearchSkills_ReturnsPagedSkillResponse()
        {
            // Arrange
            var request = new SearchSkillsCommand
            {
                Name = "C#",
                PageNumber = 1,
                PageSize = 10
            };

            var skills = new List<SkillResponse> // Ensure this is List<SkillResponse>
{
    new SkillResponse(Guid.NewGuid(), "C# Development", "Advanced C#"),
    new SkillResponse(Guid.NewGuid(), "ASP.NET", "Web development with .NET")
};

            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchSkillSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(skills); // Now returning List<SkillResponse>, which matches the repository method signature

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchSkillSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(skills.Count);

            // Act: Call the handler.
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert:
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            // Optionally, verify that the returned SkillResponse items match the domain objects.
            // (Assuming SkillResponse contains Name and RubricDescription mapped from Skill.Name and Skill.Description.)
            Assert.Contains(result.Items, item => item.Name == "C# Development" && item.Description == "Advanced C#");
            Assert.Contains(result.Items, item => item.Name == "ASP.NET" && item.Description == "Web development with .NET");

            // Verify that repository methods were called exactly once.
            _readRepositoryMock.Verify(repo => repo.ListAsync(It.IsAny<SearchSkillSpecs>(), It.IsAny<CancellationToken>()), Times.Once);
            _readRepositoryMock.Verify(repo => repo.CountAsync(It.IsAny<SearchSkillSpecs>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSkill_ReturnsUpdatedSkillResponse()
        {
            var existingSkill = Skill.Create("Old C#", "Old description");
            var skillId = existingSkill.Id;
            var request = new UpdateSkillCommand(skillId, "Updated C#", "Updated description");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(skillId, It.IsAny<CancellationToken>())).ReturnsAsync(existingSkill);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingSkill, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(skillId, result.Id);
        }

        [Fact]
        public async Task UpdateSkill_ThrowsExceptionIfNotFound()
        {
            var skillId = Guid.NewGuid();
            var request = new UpdateSkillCommand(skillId, "Updated C#", "Updated description");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(skillId, It.IsAny<CancellationToken>())).ReturnsAsync((Skill)null);

            await Assert.ThrowsAsync<SkillNotFoundException>(() => _updateHandler.Handle(request, CancellationToken.None));
        }
    }
}
