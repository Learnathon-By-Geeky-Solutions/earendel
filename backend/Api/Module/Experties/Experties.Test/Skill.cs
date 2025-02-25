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

namespace TalentMesh.Module.Experties.Tests
{
    public class SkillHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public SkillHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateSkill_ReturnsSkillResponse()
        {
            var request = new CreateSkillCommand("C# Development", "Advanced C# programming");
            var expectedId = Guid.NewGuid();
            var response = new CreateSkillResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateSkillResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateSkillCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSkill_DeletesSuccessfully()
        {
            var skillId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteSkillCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mediatorMock.Object.Send(new DeleteSkillCommand(skillId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteSkillCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSkill_ThrowsExceptionIfNotFound()
        {
            var skillId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteSkillCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SkillNotFoundException(skillId));

            var exception = await Assert.ThrowsAsync<SkillNotFoundException>(() => _mediatorMock.Object.Send(new DeleteSkillCommand(skillId)));

            Assert.NotNull(exception);
            Assert.IsType<SkillNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteSkillCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSkill_ReturnsSkillResponse()
        {
            var skillId = Guid.NewGuid();
            var expectedName = "C# Development";
            var expectedDescription = "Advanced C#";
            var skillResponse = new SkillResponse(skillId, expectedName, expectedDescription);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSkillRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(skillResponse);

            var result = await _mediatorMock.Object.Send(new GetSkillRequest(skillId));

            Assert.NotNull(result);
            Assert.Equal(skillId, result.Id);
            Assert.Equal(expectedName, result.Name);
            Assert.Equal(expectedDescription, result.Description);
            Assert.IsType<SkillResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetSkillRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSkill_ThrowsExceptionIfNotFound()
        {
            var skillId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSkillRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SkillNotFoundException(skillId));

            var exception = await Assert.ThrowsAsync<SkillNotFoundException>(() => _mediatorMock.Object.Send(new GetSkillRequest(skillId)));

            Assert.NotNull(exception);
            Assert.IsType<SkillNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetSkillRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchSkills_ReturnsPagedSkillResponse()
        {
            var request = new SearchSkillsCommand
            {
                Name = "C#",
                Description = "Advanced C#",
                PageNumber = 1,
                PageSize = 10
            };

            var pagedList = new PagedList<SkillResponse>(
                new[]
                {
                    new SkillResponse(Guid.NewGuid(), "C# Development", "Advanced C#"),
                    new SkillResponse(Guid.NewGuid(), "ASP.NET", "Web development with .NET")
                },
                1,
                10,
                2);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchSkillsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.All(result.Items, item => Assert.NotNull(item.Name));
            Assert.IsType<PagedList<SkillResponse>>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchSkillsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSkill_ReturnsUpdatedSkillResponse()
        {
            var skillId = Guid.NewGuid();
            var request = new UpdateSkillCommand(skillId, "Updated C#", "Updated description");
            var response = new UpdateSkillResponse(skillId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateSkillCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(skillId, result.Id);
            Assert.IsType<UpdateSkillResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateSkillCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSkill_ThrowsExceptionIfNotFound()
        {
            var skillId = Guid.NewGuid();
            var request = new UpdateSkillCommand(skillId, "Updated C#", "Updated description");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateSkillCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SkillNotFoundException(skillId));

            var exception = await Assert.ThrowsAsync<SkillNotFoundException>(() => _mediatorMock.Object.Send(request));

            Assert.NotNull(exception);
            Assert.IsType<SkillNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateSkillCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
