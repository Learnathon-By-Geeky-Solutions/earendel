using Moq;
using TalentMesh.Module.Experties.Application.Rubrics.Create.v1;
using TalentMesh.Module.Experties.Application.Rubrics.Delete.v1;
using TalentMesh.Module.Experties.Application.Rubrics.Get.v1;
using TalentMesh.Module.Experties.Application.Rubrics.Search.v1;
using TalentMesh.Module.Experties.Application.Rubrics.Update.v1;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;

namespace TalentMesh.Module.Experties.Tests
{
    public class RubricHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public RubricHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateRubric_ReturnsRubricResponse()
        {
            var subSkillId = Guid.NewGuid();
            var seniorityLevelId = Guid.NewGuid();
            var request = new CreateRubricCommand(subSkillId, seniorityLevelId, "Effective C#", "C# advanced topics", 0.8m);
            var expectedId = Guid.NewGuid();
            var response = new CreateRubricResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateRubricResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateRubricCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRubric_DeletesSuccessfully()
        {
            var rubricId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteRubricCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mediatorMock.Object.Send(new DeleteRubricCommand(rubricId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteRubricCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRubric_ThrowsExceptionIfNotFound()
        {
            var rubricId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteRubricCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RubricNotFoundException(rubricId));

            var exception = await Assert.ThrowsAsync<RubricNotFoundException>(() => _mediatorMock.Object.Send(new DeleteRubricCommand(rubricId)));

            Assert.NotNull(exception);
            Assert.IsType<RubricNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteRubricCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetRubric_ReturnsRubricResponse()
        {
            var rubricId = Guid.NewGuid();
            var subSkillId = Guid.NewGuid();
            var seniorityLevelId = Guid.NewGuid();
            var expectedTitle = "Effective C#";
            var expectedDescription = "C# advanced topics";
            var expectedWeight = 0.8m;
            var rubricResponse = new RubricResponse(rubricId, expectedTitle, expectedDescription, subSkillId, seniorityLevelId, expectedWeight);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetRubricRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rubricResponse);

            var result = await _mediatorMock.Object.Send(new GetRubricRequest(rubricId));

            Assert.NotNull(result);
            Assert.Equal(rubricId, result.Id);
            Assert.Equal(expectedTitle, result.Title);
            Assert.Equal(expectedDescription, result.RubricDescription);
            Assert.Equal(subSkillId, result.SubSkillId);
            Assert.Equal(seniorityLevelId, result.SeniorityLevelId);
            Assert.Equal(expectedWeight, result.Weight);
            Assert.IsType<RubricResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetRubricRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetRubric_ThrowsExceptionIfNotFound()
        {
            var rubricId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetRubricRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RubricNotFoundException(rubricId));

            var exception = await Assert.ThrowsAsync<RubricNotFoundException>(() => _mediatorMock.Object.Send(new GetRubricRequest(rubricId)));

            Assert.NotNull(exception);
            Assert.IsType<RubricNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetRubricRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchRubrics_ReturnsPagedRubricResponse()
        {
            var request = new SearchRubricsCommand { Title = "Effective", PageNumber = 1, PageSize = 10 };
            var rubric1 = new RubricResponse(Guid.NewGuid(), "Effective C#", "C# advanced topics", Guid.NewGuid(), Guid.NewGuid(), 0.8m);
            var rubric2 = new RubricResponse(Guid.NewGuid(), "Effective Java", "Java advanced topics", Guid.NewGuid(), Guid.NewGuid(), 0.9m);
            var pagedList = new PagedList<RubricResponse>(new[] { rubric1, rubric2 }, 1, 10, 2);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchRubricsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal("Effective C#", result.Items[0].Title);
            Assert.Equal("Effective Java", result.Items[1].Title);
            Assert.IsType<PagedList<RubricResponse>>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchRubricsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRubric_ReturnsUpdatedRubricResponse()
        {
            var rubricId = Guid.NewGuid();
            var subSkillId = Guid.NewGuid();
            var seniorityLevelId = Guid.NewGuid();
            var request = new UpdateRubricCommand(rubricId, subSkillId, seniorityLevelId, 1.0m, "Updated Effective C#", "Updated description");
            var response = new UpdateRubricResponse(rubricId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateRubricCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(rubricId, result.Id);
            Assert.IsType<UpdateRubricResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateRubricCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRubric_ThrowsExceptionIfNotFound()
        {
            var rubricId = Guid.NewGuid();
            var subSkillId = Guid.NewGuid();
            var seniorityLevelId = Guid.NewGuid();
            var request = new UpdateRubricCommand(rubricId, subSkillId, seniorityLevelId, 1.0m, "Updated Effective C#", "Updated description");
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateRubricCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RubricNotFoundException(rubricId));

            var exception = await Assert.ThrowsAsync<RubricNotFoundException>(() => _mediatorMock.Object.Send(request));

            Assert.NotNull(exception);
            Assert.IsType<RubricNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateRubricCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
