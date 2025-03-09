using Moq;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Create.v1;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Delete.v1;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Get.v1;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Search.v1;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Update.v1;
using TalentMesh.Module.Candidate.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;
using Xunit;

namespace TalentMesh.Module.Candidate.Tests
{
    public class CandidateProfileHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public CandidateProfileHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateCandidateProfile_ReturnsCandidateProfileResponse()
        {
            // Arrange
            var request = new CreateCandidateProfileCommand(
                UserId: Guid.NewGuid(),
                Resume: "Sample resume",
                Skills: "C#, ASP.NET Core",
                Experience: "3 years experience",
                Education: "Bachelor's in Computer Science"
            );
            var expectedId = Guid.NewGuid();
            var response = new CreateCandidateProfileResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.IsType<CreateCandidateProfileResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateCandidateProfileCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCandidateProfile_DeletesSuccessfully()
        {
            // Arrange
            var candidateProfileId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteCandidateProfileCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _mediatorMock.Object.Send(new DeleteCandidateProfileCommand(candidateProfileId));

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteCandidateProfileCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCandidateProfile_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var candidateProfileId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteCandidateProfileCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CandidateProfileNotFoundException(candidateProfileId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CandidateProfileNotFoundException>(
                () => _mediatorMock.Object.Send(new DeleteCandidateProfileCommand(candidateProfileId))
            );
            Assert.NotNull(exception);
            Assert.IsType<CandidateProfileNotFoundException>(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteCandidateProfileCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCandidateProfile_ReturnsCandidateProfileResponse()
        {
            // Arrange
            var candidateProfileId = Guid.NewGuid();
            var resume = "Sample resume";
            var skills = "C#, SQL";
            var experience = "5 years";
            var education = "Master's in Software Engineering";
            // The last parameter is the associated user Id.
            var candidateProfileResponse = new CandidateProfileResponse(candidateProfileId, resume, skills, experience, education, Guid.NewGuid());

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCandidateProfileRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidateProfileResponse);

            // Act
            var result = await _mediatorMock.Object.Send(new GetCandidateProfileRequest(candidateProfileId));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(candidateProfileId, result.Id);
            Assert.Equal(resume, result.Resume);
            Assert.Equal(skills, result.Skills);
            Assert.Equal(experience, result.Experience);
            Assert.Equal(education, result.Education);
            Assert.IsType<CandidateProfileResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetCandidateProfileRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCandidateProfile_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var candidateProfileId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCandidateProfileRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CandidateProfileNotFoundException(candidateProfileId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CandidateProfileNotFoundException>(
                () => _mediatorMock.Object.Send(new GetCandidateProfileRequest(candidateProfileId))
            );
            Assert.NotNull(exception);
            Assert.IsType<CandidateProfileNotFoundException>(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetCandidateProfileRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchCandidateProfiles_ReturnsPagedCandidateProfileResponse()
        {
            // Arrange
            var request = new SearchCandidateProfileCommand
            {
                Resume = "Sample",
                PageNumber = 1,
                PageSize = 10
            };
            var candidate1 = new CandidateProfileResponse(Guid.NewGuid(), "Sample resume", "C#, ASP.NET Core", "3 years", "Bachelor's", Guid.NewGuid());
            var candidate2 = new CandidateProfileResponse(Guid.NewGuid(), "Sample resume", "SQL, NoSQL", "5 years", "Master's", Guid.NewGuid());
            var pagedList = new PagedList<CandidateProfileResponse>(new[] { candidate1, candidate2 }, 1, 10, 2);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchCandidateProfileCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal("Sample resume", result.Items[0].Resume);
            Assert.Equal("Sample resume", result.Items[1].Resume);
            Assert.IsType<PagedList<CandidateProfileResponse>>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchCandidateProfileCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCandidateProfile_ReturnsUpdatedCandidateProfileResponse()
        {
            // Arrange
            var candidateProfileId = Guid.NewGuid();
            var request = new UpdateCandidateProfileCommand(candidateProfileId, "Updated resume", "Updated skills", "Updated experience", "Updated education");
            var response = new UpdateCandidateProfileResponse(candidateProfileId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateCandidateProfileCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _mediatorMock.Object.Send(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(candidateProfileId, result.Id);
            Assert.IsType<UpdateCandidateProfileResponse>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateCandidateProfileCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCandidateProfile_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var candidateProfileId = Guid.NewGuid();
            var request = new UpdateCandidateProfileCommand(candidateProfileId, "Updated resume", "Updated skills", "Updated experience", "Updated education");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateCandidateProfileCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CandidateProfileNotFoundException(candidateProfileId));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CandidateProfileNotFoundException>(
                () => _mediatorMock.Object.Send(request)
            );
            Assert.NotNull(exception);
            Assert.IsType<CandidateProfileNotFoundException>(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateCandidateProfileCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
