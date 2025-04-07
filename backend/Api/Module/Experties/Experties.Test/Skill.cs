using Moq;
using Xunit;
using TalentMesh.Module.Experties.Application.Skills.Create.v1;
using TalentMesh.Module.Experties.Application.Skills.Delete.v1;
using TalentMesh.Module.Experties.Application.Skills.Get.v1;
using TalentMesh.Module.Experties.Application.Skills.Search.v1;
using TalentMesh.Module.Experties.Application.Skills.Update.v1;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Framework.Infrastructure.Messaging;
using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Create.v1;

using System.Reflection;

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
        private readonly Mock<IMessageBus> _messageBusMock;
        private readonly Mock<IMediator> _mediatorMock;
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

            _messageBusMock = new Mock<IMessageBus>();
            _mediatorMock = new Mock<IMediator>(); // Initialize the mediator mock

            _createHandler = new CreateSkillHandler(_createLoggerMock.Object, _repositoryMock.Object, _messageBusMock.Object, _mediatorMock.Object);
            _deleteHandler = new DeleteSkillHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetSkillHandler(_readRepositoryMock.Object, _cacheServiceMock.Object); // Correct parameters
            _searchHandler = new SearchSkillsHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateSkillHandler(_updateLoggerMock.Object, _repositoryMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task CreateSkill_WithSeniorityLevels_ReturnsSkillResponseAndCreatesJunction()
        {
            // Arrange
            var seniorityLevelIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            var request = new CreateSkillCommand(
                "C# Development",
                "Advanced C# programming",
                seniorityLevelIds // Non-empty SeniorityLevels
            );

            var expectedSkill = Skill.Create(request.Name!, request.Description);

            _repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Skill>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedSkill);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateSeniorityLevelJunctionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateSeniorityLevelJunctionResponse(seniorityLevelIds));

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Skill>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateSeniorityLevelJunctionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateSkill_WithoutSeniorityLevels_ReturnsSkillResponseAndSkipsJunction()
        {
            // Arrange
            var request = new CreateSkillCommand(
                "C# Development",
                "Advanced C# programming",
                null // No SeniorityLevels (or use new List<Guid>())
            );

            var expectedSkill = Skill.Create(request.Name!, request.Description);
            _repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Skill>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedSkill);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Skill>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateSeniorityLevelJunctionCommand>(), It.IsAny<CancellationToken>()), Times.Never);
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

            // Add SubSkill
            var subSkill = SubSkill.Create("LINQ", "Language Queries", skill.Id);
            skill.SubSkills.Add(subSkill);

            // Create Seniority and Junction
            var seniority = Seniority.Create("Senior", "Senior developer");
            var junction = SeniorityLevelJunction.Create(seniority.Id, skill.Id);

            // Set navigation property using CORRECT reflection
            var seniorityProp = typeof(SeniorityLevelJunction)
                               .GetProperty("Seniority", BindingFlags.Public | BindingFlags.Instance);
            seniorityProp?.SetValue(junction, seniority);

            skill.SeniorityLevelJunctions.Add(junction);

            // Mock repository response
            _readRepositoryMock.Setup(repo =>
                repo.GetBySpecAsync(It.IsAny<GetSkillSpec>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(skill);

            // Act
            var result = await _getHandler.Handle(new GetSkillRequest(skill.Id), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(skill.Id, result.Id);

            var junctionResponse = result.SeniorityLevelJunctions.First();
            Assert.Equal(seniority.Id, junctionResponse.SeniorityLevelId);
            Assert.Equal("Senior", junctionResponse.Seniority.Name); // Full validation
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

            var skills = new List<SkillResponse>
            {
                new SkillResponse(Guid.NewGuid(), "C# Development", "Advanced C#", new List<SubSkillResponse>(), new List<SeniorityLevelJunctionResponse>()),
                new SkillResponse(Guid.NewGuid(), "ASP.NET", "Web development with .NET", new List<SubSkillResponse>(), new List<SeniorityLevelJunctionResponse>())
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
            var seniorityLevelIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            var request = new UpdateSkillCommand(skillId, "Updated C#", "Updated description", seniorityLevelIds);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(skillId, It.IsAny<CancellationToken>())).ReturnsAsync(existingSkill);
            _repositoryMock.Setup(repo => repo.UpdateAsync(existingSkill, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(skillId, result.Id);
        }


        [Fact]
        public async Task UpdateSkill_WhenFieldsChanged_ShouldUpdate()
        {
            var existingSeniority = Skill.Create("Old Title", "Old Desc");
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSkillCommand(seniorityId, "New Title", "New Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(seniorityId, result.Id);

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Skill>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task UpdateSkill_WhenFieldsUnchanged_ShouldStillReturnButNotReassign()
        {
            var existingSeniority = Skill.Create("Same Title", "Same Desc");
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSkillCommand(seniorityId, "Same Title", "Same Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(seniorityId, result.Id);

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Skill>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSkill_WhenNewTitleIsDifferent_ShouldUpdateTitle()
        {
            // Arrange
            var existingSeniority = Skill.Create("Old Title", "Old Desc");
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSkillCommand(seniorityId, "New Title", "Old Desc");

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
        public async Task UpdateSkill_WhenNewTitleIsSame_ShouldNotUpdateTitle()
        {
            // Arrange
            var existingSeniority = Skill.Create("Old Title", "Old Desc");
            var seniorityId = existingSeniority.Id;
            // Using a different casing but the same value
            var request = new UpdateSkillCommand(seniorityId, "old title", "Old Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // The title remains unchanged because "old title" equals "Old Title" (ignoring case)
            Assert.Equal("Old Title", existingSeniority.Name);
        }

        // 3. When new title is null → update block is skipped.
        [Fact]
        public async Task UpdateSkill_WhenNewTitleIsNull_ShouldNotUpdateTitle()
        {
            // Arrange
            var existingSeniority = Skill.Create("Old Title", "Old Desc");
            var seniorityId = existingSeniority.Id;
            // Passing null for title means no change should occur.
            var request = new UpdateSkillCommand(seniorityId, null, "Updated Desc");

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
        public async Task UpdateSkill_WhenCurrentTitleIsNullAndNewTitleIsNonNull_ShouldUpdateTitle()
        {
            // Arrange
            // Create a seniority with a non-null title then force the title to null for test purposes.
            var existingSeniority = Skill.Create(null, "Old Desc");
            // Forcing the Name to null (assuming Name has an accessible setter in test context)
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSkillCommand(seniorityId, "New Title", "Old Desc");

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
        public async Task UpdateSkill_WhenNewDescriptionIsDifferent_ShouldUpdateDescription()
        {
            var existingSeniority = Skill.Create("Any Title", "Old Desc");
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSkillCommand(seniorityId, "Any Title", "New Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("New Desc", existingSeniority.Description);
        }

        [Fact]
        public async Task UpdateSkill_WhenNewDescriptionIsSame_ShouldNotUpdateDescription()
        {
            var existingSeniority = Skill.Create("Any Title", "Same Desc");
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSkillCommand(seniorityId, "Any Title", "same desc"); // same but lowercase

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Same Desc", existingSeniority.Description); // unchanged
        }

        [Fact]
        public async Task UpdateSkill_WhenNewDescriptionIsNull_ShouldNotUpdateDescription()
        {
            var existingSeniority = Skill.Create("Any Title", "Existing Desc");
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSkillCommand(seniorityId, "Any Title", null); // null desc

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Existing Desc", existingSeniority.Description); // unchanged
        }

        [Fact]
        public async Task UpdateSkill_WhenCurrentDescriptionIsNullAndNewIsProvided_ShouldUpdateDescription()
        {
            var existingSeniority = Skill.Create("Any Title", null);
            var seniorityId = existingSeniority.Id;
            var request = new UpdateSkillCommand(seniorityId, "Any Title", "Provided Desc");

            _repositoryMock.Setup(r => r.GetByIdAsync(seniorityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSeniority);
            _repositoryMock.Setup(r => r.UpdateAsync(existingSeniority, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _updateHandler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Provided Desc", existingSeniority.Description);
        }


        // [Fact]
        // public async Task UpdateSkill_WithoutSeniorityIds()
        // {
        //     var existingSkill = Skill.Create("Old C#", "Old description");
        //     var skillId = existingSkill.Id;

        //     var request = new UpdateSkillCommand(skillId, "Updated C#", "Updated description", null);

        //     _repositoryMock.Setup(repo => repo.GetByIdAsync(skillId, It.IsAny<CancellationToken>())).ReturnsAsync(existingSkill);
        //     _repositoryMock.Setup(repo => repo.UpdateAsync(existingSkill, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        //     var result = await _updateHandler.Handle(request, CancellationToken.None);

        //     Assert.NotNull(result);
        //     Assert.Equal(skillId, result.Id);
        // }

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
