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
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Candidate.Domain;
using TalentMesh.Framework.Core.Caching;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Candidate.Tests
{
    public class CandidateProfileHandlerTests
    {
        private readonly Mock<IRepository<CandidateProfile>> _repositoryMock;
        private readonly Mock<IReadRepository<CandidateProfile>> _readRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<CreateCandidateProfileHandler>> _createLoggerMock;
        private readonly Mock<ILogger<DeleteCandidateProfileHandler>> _deleteLoggerMock;
        private readonly Mock<ILogger<GetCandidateProfileHandler>> _getLoggerMock;
        private readonly Mock<ILogger<SearchCandidateProfileHandler>> _searchLoggerMock;
        private readonly Mock<ILogger<UpdateCandidateProfileHandler>> _updateLoggerMock;

        private readonly CreateCandidateProfileHandler _createHandler;
        private readonly DeleteCandidateProfileHandler _deleteHandler;
        private readonly GetCandidateProfileHandler _getHandler;
        private readonly SearchCandidateProfileHandler _searchHandler;
        private readonly UpdateCandidateProfileHandler _updateHandler;

        public CandidateProfileHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<CandidateProfile>>();
            _readRepositoryMock = new Mock<IReadRepository<CandidateProfile>>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createLoggerMock = new Mock<ILogger<CreateCandidateProfileHandler>>();
            _deleteLoggerMock = new Mock<ILogger<DeleteCandidateProfileHandler>>();
            _getLoggerMock = new Mock<ILogger<GetCandidateProfileHandler>>();
            _searchLoggerMock = new Mock<ILogger<SearchCandidateProfileHandler>>();
            _updateLoggerMock = new Mock<ILogger<UpdateCandidateProfileHandler>>();

            _createHandler = new CreateCandidateProfileHandler(_createLoggerMock.Object, _repositoryMock.Object);
            _deleteHandler = new DeleteCandidateProfileHandler(_deleteLoggerMock.Object, _repositoryMock.Object);
            _getHandler = new GetCandidateProfileHandler(_readRepositoryMock.Object, _cacheServiceMock.Object);
            _searchHandler = new SearchCandidateProfileHandler(_readRepositoryMock.Object);
            _updateHandler = new UpdateCandidateProfileHandler(_updateLoggerMock.Object, _repositoryMock.Object);

        }

        [Fact]
        public async Task CreateCandidateProfile_ReturnsCandidateProfileResponse()
        {
            // Arrange
            var subSkillId = Guid.NewGuid();
            var seniorityLevelId = Guid.NewGuid();
            var request = new CreateCandidateProfileCommand("resume", "skills", "experience", "education", Guid.NewGuid());
            var expectedCandidateProfile = CandidateProfile.Create(request.UserId, request.Resume!, request.Skills!, request.Experience, request.Education);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<CandidateProfile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCandidateProfile);

            // Act
            var result = await _createHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<CandidateProfile>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCandidateProfile_DeletesSuccessfully()
        {
            // Arrange
            var existingCandidateProfile = CandidateProfile.Create(Guid.NewGuid(), "resume", "skills", "experience", "education");
            var CandidateProfileId = existingCandidateProfile.Id;

            _repositoryMock.Setup(repo => repo.GetByIdAsync(CandidateProfileId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCandidateProfile);

            // Act
            await _deleteHandler.Handle(new DeleteCandidateProfileCommand(CandidateProfileId), CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(existingCandidateProfile, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(CandidateProfileId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCandidateProfile_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var CandidateProfileId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(CandidateProfileId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CandidateProfile)null);

            // Act & Assert
            await Assert.ThrowsAsync<CandidateProfileNotFoundException>(() =>
                _deleteHandler.Handle(new DeleteCandidateProfileCommand(CandidateProfileId), CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(CandidateProfileId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCandidateProfile_ReturnsCandidateProfileResponse()
        {
            // Arrange
            var expectedCandidateProfile = CandidateProfile.Create(Guid.NewGuid(), "resume", "skills", "experience", "education");
            var CandidateProfileId = expectedCandidateProfile.Id;

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(CandidateProfileId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCandidateProfile);

            _cacheServiceMock.Setup(cache => cache.GetAsync<CandidateProfileResponse>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CandidateProfileResponse)null);

            // Act
            var result = await _getHandler.Handle(new GetCandidateProfileRequest(CandidateProfileId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCandidateProfile.Id, result.Id);
            Assert.Equal(expectedCandidateProfile.UserId, result.UserId);
            Assert.Equal(expectedCandidateProfile.Resume, result.Resume);
            Assert.Equal(expectedCandidateProfile.Skills, result.Skills);

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(CandidateProfileId, It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<CandidateProfileResponse>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCandidateProfile_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var CandidateProfileId = Guid.NewGuid();

            _readRepositoryMock.Setup(repo => repo.GetByIdAsync(CandidateProfileId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CandidateProfile)null);

            // Act & Assert
            await Assert.ThrowsAsync<CandidateProfileNotFoundException>(() =>
                _getHandler.Handle(new GetCandidateProfileRequest(CandidateProfileId), CancellationToken.None));

            _readRepositoryMock.Verify(repo => repo.GetByIdAsync(CandidateProfileId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchCandidateProfiles_ReturnsPagedCandidateProfileResponse()
        {
            // Arrange
            var request = new SearchCandidateProfileCommand
            {
                UserId = Guid.NewGuid(),
                Skills = "skills",
                PageSize = 10
            };

            var CandidateProfiles = new List<CandidateProfileResponse>
            {
                new CandidateProfileResponse(Guid.NewGuid(), "resume", "skills", "experience", "education", Guid.NewGuid()),
            };
            var totalCount = CandidateProfiles.Count;

            // Mock returns List<CandidateProfile> (domain entities)
            _readRepositoryMock
                .Setup(repo => repo.ListAsync(It.IsAny<SearchCandidateProfileSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(CandidateProfiles);

            _readRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<SearchCandidateProfileSpecs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalCount);

            // Act
            var result = await _searchHandler.Handle(request, CancellationToken.None);

            // Assert: Verify mapped DTOs
            Assert.NotNull(result);
            Assert.Equal(1, result.Items.Count);

            Assert.Contains(result.Items, item =>
                item.Resume == "resume" &&
                item.Skills == "skills"
            );

            // Verify repository calls
            _readRepositoryMock.Verify(repo =>
                repo.ListAsync(It.IsAny<SearchCandidateProfileSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _readRepositoryMock.Verify(repo =>
                repo.CountAsync(It.IsAny<SearchCandidateProfileSpecs>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Fact]
        public async Task UpdateCandidateProfile_ReturnsUpdatedCandidateProfileResponse()
        {
            // Arrange
            var existingCandidateProfile = CandidateProfile.Create(Guid.NewGuid(), "resume", "skills", "experience", "education");
            var CandidateProfileId = existingCandidateProfile.Id;
            var request = new UpdateCandidateProfileCommand(
                CandidateProfileId,
                "resume",
                "skills",
                "experience",
                "education"
            );

            _repositoryMock.Setup(repo => repo.GetByIdAsync(CandidateProfileId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCandidateProfile);

            // Act
            var result = await _updateHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(CandidateProfileId, result.Id);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(CandidateProfileId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<CandidateProfile>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCandidateProfile_ThrowsExceptionIfNotFound()
        {
            // Arrange
            var CandidateProfileId = Guid.NewGuid();
            var request = new UpdateCandidateProfileCommand(CandidateProfileId, "resume", "skills", "experience", "education");

            _repositoryMock.Setup(repo => repo.GetByIdAsync(CandidateProfileId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CandidateProfile)null);

            // Act & Assert
            await Assert.ThrowsAsync<CandidateProfileNotFoundException>(() =>
                _updateHandler.Handle(request, CancellationToken.None));

            _repositoryMock.Verify(repo => repo.GetByIdAsync(CandidateProfileId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
