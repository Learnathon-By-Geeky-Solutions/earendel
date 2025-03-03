using Moq;
using Xunit;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Create.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Delete.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Search.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Update.v1;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;

namespace TalentMesh.Module.Experties.Tests
{
    public class SeniorityLevelJunctionHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public SeniorityLevelJunctionHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateSeniorityLevelJunction_ReturnsSeniorityLevelJunctionResponse()
        {
            var seniorityLevelId = Guid.NewGuid();
            var skillId = Guid.NewGuid();
            var request = new CreateSeniorityLevelJunctionCommand(seniorityLevelId, skillId);
            var expectedId = Guid.NewGuid();
            var response = new CreateSeniorityLevelJunctionResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateSeniorityLevelJunctionResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateSeniorityLevelJunctionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSeniorityLevelJunction_DeletesSuccessfully()
        {
            var junctionId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteSeniorityLevelJunctionCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mediatorMock.Object.Send(new DeleteSeniorityLevelJunctionCommand(junctionId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteSeniorityLevelJunctionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSeniorityLevelJunction_ReturnsSeniorityLevelJunctionResponse()
        {
            var junctionId = Guid.NewGuid();
            var seniorityLevelId = Guid.NewGuid();
            var skillId = Guid.NewGuid();
            var junctionResponse = new SeniorityLevelJunctionResponse(junctionId, seniorityLevelId, skillId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSeniorityLevelJunctionRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(junctionResponse);

            var result = await _mediatorMock.Object.Send(new GetSeniorityLevelJunctionRequest(junctionId));

            Assert.NotNull(result);
            Assert.Equal(junctionId, result.Id);
            Assert.Equal(seniorityLevelId, result.SeniorityLevelId);
            Assert.Equal(skillId, result.SkillId);
            Assert.IsType<SeniorityLevelJunctionResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetSeniorityLevelJunctionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchSeniorityLevelJunctions_ReturnsPagedSeniorityLevelJunctionResponse()
        {
            var request = new SearchSeniorityLevelJunctionCommand
            {
                SeniorityLevelId = Guid.NewGuid(),
                SkillId = Guid.NewGuid(),
                PageNumber = 1,
                PageSize = 10
            };

            var pagedList = new PagedList<SeniorityLevelJunctionResponse>(
                new[]
                {
                    new SeniorityLevelJunctionResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
                    new SeniorityLevelJunctionResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                },
                1,
                10,
                2);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchSeniorityLevelJunctionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.IsType<PagedList<SeniorityLevelJunctionResponse>>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchSeniorityLevelJunctionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSeniorityLevelJunction_ReturnsUpdatedSeniorityLevelJunctionResponse()
        {
            var junctionId = Guid.NewGuid();
            var request = new UpdateSeniorityLevelJunctionCommand(junctionId, Guid.NewGuid(), Guid.NewGuid());
            var response = new UpdateSeniorityLevelJunctionResponse(junctionId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateSeniorityLevelJunctionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(junctionId, result.Id);
            Assert.IsType<UpdateSeniorityLevelJunctionResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateSeniorityLevelJunctionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}