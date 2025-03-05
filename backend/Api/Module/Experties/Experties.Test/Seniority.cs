using Moq;
using Xunit;
using TalentMesh.Module.Experties.Application.Seniorities.Create.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Delete.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Search.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Update.v1;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;

namespace TalentMesh.Module.Experties.Tests
{
    public class SeniorityHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public SeniorityHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateSeniority_ReturnsSeniorityResponse()
        {
            var request = new CreateSeniorityCommand("Internship", "0 Years of Experience");
            var expectedId = Guid.NewGuid();
            var response = new CreateSeniorityResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateSeniorityResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateSeniorityCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSeniority_DeletesSuccessfully()
        {
            var seniorityId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteSeniorityCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mediatorMock.Object.Send(new DeleteSeniorityCommand(seniorityId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteSeniorityCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSeniority_ThrowsExceptionIfNotFound()
        {
            var seniorityId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteSeniorityCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SeniorityNotFoundException(seniorityId));

            var exception = await Assert.ThrowsAsync<SeniorityNotFoundException>(() => _mediatorMock.Object.Send(new DeleteSeniorityCommand(seniorityId)));

            Assert.NotNull(exception);
            Assert.IsType<SeniorityNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteSeniorityCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSeniority_ReturnsSeniorityResponse()
        {
            var seniorityId = Guid.NewGuid();
            var expectedName = "Internship";
            var expectedDescription = "0 Years of Experience";
            var seniorityResponse = new SeniorityResponse(seniorityId, expectedName, expectedDescription);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSeniorityRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(seniorityResponse);

            var result = await _mediatorMock.Object.Send(new GetSeniorityRequest(seniorityId));

            Assert.NotNull(result);
            Assert.Equal(seniorityId, result.Id);
            Assert.Equal(expectedName, result.Name);
            Assert.Equal(expectedDescription, result.Description);
            Assert.IsType<SeniorityResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetSeniorityRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSeniority_ThrowsExceptionIfNotFound()
        {
            var seniorityId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSeniorityRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SeniorityNotFoundException(seniorityId));

            var exception = await Assert.ThrowsAsync<SeniorityNotFoundException>(() => _mediatorMock.Object.Send(new GetSeniorityRequest(seniorityId)));

            Assert.NotNull(exception);
            Assert.IsType<SeniorityNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetSeniorityRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchSeniorities_ReturnsPagedSeniorityResponse()
        {
            var request = new SearchSenioritiesCommand
            {
                Name = "Internship",
                Description = "0 Years of Experience",
                PageNumber = 1,
                PageSize = 10
            };

            var pagedList = new PagedList<SeniorityResponse>(
                new[]
                {
                    new SeniorityResponse(Guid.NewGuid(), "Internship", "0 Years of Experience"),
                    new SeniorityResponse(Guid.NewGuid(), "Entry-Level", "1-2 Years of Experience")
                },
                1,
                10,
                2);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchSenioritiesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.All(result.Items, item => Assert.NotNull(item.Name));
            Assert.IsType<PagedList<SeniorityResponse>>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchSenioritiesCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSeniority_ReturnsUpdatedSeniorityResponse()
        {
            var seniorityId = Guid.NewGuid();
            var request = new UpdateSeniorityCommand(seniorityId, "Updated Internship", "Updated 0 Years of Experience");
            var response = new UpdateSeniorityResponse(seniorityId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateSeniorityCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(seniorityId, result.Id);
            Assert.IsType<UpdateSeniorityResponse>(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateSeniorityCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSeniority_ThrowsExceptionIfNotFound()
        {
            var seniorityId = Guid.NewGuid();
            var request = new UpdateSeniorityCommand(seniorityId, "Updated Internship", "Updated 0 Years of Experience");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateSeniorityCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SeniorityNotFoundException(seniorityId));

            var exception = await Assert.ThrowsAsync<SeniorityNotFoundException>(() => _mediatorMock.Object.Send(request));

            Assert.NotNull(exception);
            Assert.IsType<SeniorityNotFoundException>(exception);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateSeniorityCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
