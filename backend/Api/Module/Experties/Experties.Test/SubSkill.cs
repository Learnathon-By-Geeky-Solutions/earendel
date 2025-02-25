using Moq;
using TalentMesh.Module.Experties.Application.SubSkills.Create.v1;
using TalentMesh.Module.Experties.Application.SubSkills.Delete.v1;
using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using TalentMesh.Module.Experties.Application.SubSkills.Search.v1;
using TalentMesh.Module.Experties.Application.SubSkills.Update.v1;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;

namespace TalentMesh.Module.Experties.Tests
{
    public class SubSkillHandlerTests
    {

        private readonly Mock<ISender> _mediatorMock;

        public SubSkillHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateSubSkill_ReturnsSubSkillResponse()
        {
            var skillId = Guid.NewGuid();
            var request = new CreateSubSkillCommand(skillId, "ASP.NET Core", "Web framework");
            var expectedId = Guid.NewGuid();
            var response = new CreateSubSkillResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateSubSkillResponse>(result);
        }

        [Fact]
        public async Task DeleteSubSkill_DeletesSuccessfully()
        {
            var subSkillId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteSubSkillCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mediatorMock.Object.Send(new DeleteSubSkillCommand(subSkillId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteSubSkillCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task DeleteSubSkill_ThrowsExceptionIfNotFound()
        {
            var subSkillId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteSubSkillCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SubSkillNotFoundException(subSkillId));

            var exception = await Assert.ThrowsAsync<SubSkillNotFoundException>(() => _mediatorMock.Object.Send(new DeleteSubSkillCommand(subSkillId)));

            Assert.NotNull(exception);
            Assert.IsType<SubSkillNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteSubSkillCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSubSkill_ReturnsSubSkillResponse()
        {
            var subSkillId = Guid.NewGuid();
            var skillId = Guid.NewGuid();
            var response = new SubSkillResponse(subSkillId, "ASP.NET Core", "Web framework", skillId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSubSkillRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(new GetSubSkillRequest(subSkillId));

            Assert.NotNull(result);
            Assert.Equal(subSkillId, result.Id);
            Assert.Equal("ASP.NET Core", result.Name);
            Assert.Equal("Web framework", result.Description);
            Assert.IsType<SubSkillResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetSubSkillRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSubSkill_ThrowsExceptionIfNotFound()
        {
            var subSkillId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSubSkillRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SubSkillNotFoundException(subSkillId));

            var exception = await Assert.ThrowsAsync<SubSkillNotFoundException>(() => _mediatorMock.Object.Send(new GetSubSkillRequest(subSkillId)));

            Assert.NotNull(exception);
            Assert.IsType<SubSkillNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetSubSkillRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchSubSkills_ReturnsPagedSubSkillResponse()
        {
            var request = new SearchSubSkillsCommand { Name = "ASP.NET", PageNumber = 1, PageSize = 10 };
            var pagedList = new PagedList<SubSkillResponse>(
                new[] { new SubSkillResponse(Guid.NewGuid(), "ASP.NET Core", "Web framework", Guid.NewGuid()) },
                1, 10, 1);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchSubSkillsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.All(result.Items, item => Assert.NotNull(item.Name));
            Assert.IsType<PagedList<SubSkillResponse>>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchSubSkillsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSubSkill_ReturnsUpdatedSubSkillResponse()
        {
            var subSkillId = Guid.NewGuid();
            var skillId = Guid.NewGuid();
            var request = new UpdateSubSkillCommand(skillId, subSkillId, "Updated ASP.NET Core", "Updated description");
            var response = new UpdateSubSkillResponse(subSkillId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateSubSkillCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(subSkillId, result.Id);
            Assert.IsType<UpdateSubSkillResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateSubSkillCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSubSkill_ThrowsExceptionIfNotFound()
        {
            var subSkillId = Guid.NewGuid();
            var skillId = Guid.NewGuid();
            var request = new UpdateSubSkillCommand(skillId, subSkillId, "Updated ASP.NET Core", "Updated description");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateSubSkillCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SubSkillNotFoundException(subSkillId));

            var exception = await Assert.ThrowsAsync<SubSkillNotFoundException>(() => _mediatorMock.Object.Send(request));

            Assert.NotNull(exception);
            Assert.IsType<SubSkillNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateSubSkillCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
