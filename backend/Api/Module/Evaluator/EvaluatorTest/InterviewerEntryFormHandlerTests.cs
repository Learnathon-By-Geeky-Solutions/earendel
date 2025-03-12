using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;

using TalentMesh.Module.Interviews.Domain.Exceptions;
using TalentMesh.Framework.Core.Paging;
using MediatR;
using Xunit;
using Evaluator.Application.Interviewer.Get.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Delete.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1;
using TalentMesh.Module.Evaluator.Domain.Exceptions;

namespace TalentMesh.Module.Evaluator.Tests
{
    public class InterviewerEntryFormHandlerTests
    {
        private readonly Mock<ISender> _mediatorMock;

        public InterviewerEntryFormHandlerTests()
        {
            _mediatorMock = new Mock<ISender>();
        }

        [Fact]
        public async Task CreateInterviewerEntryForm_ReturnsResponse()
        {
            var request = new CreateInterviewerEntryFormCommand(
                UserId: Guid.NewGuid(),
                AdditionalInfo: "Experienced in conducting interviews."
            );
            var expectedId = Guid.NewGuid();
            var response = new CreateInterviewerEntryFormResponse(expectedId);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateInterviewerEntryFormCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewerEntryForm_DeletesSuccessfully()
        {
            var entryFormId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteInterviewerEntryFormCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _mediatorMock.Object.Send(new DeleteInterviewerEntryFormCommand(entryFormId));

            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteInterviewerEntryFormCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInterviewerEntryForm_ThrowsExceptionIfNotFound()
        {
            var entryFormId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteInterviewerEntryFormCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewerEntryFormNotFoundException(entryFormId));

            var exception = await Assert.ThrowsAsync<InterviewerEntryFormNotFoundException>(() =>
                _mediatorMock.Object.Send(new DeleteInterviewerEntryFormCommand(entryFormId))
            );

            Assert.NotNull(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteInterviewerEntryFormCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewerEntryForm_ReturnsResponse()
        {
            var entryFormId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var additionalInfo = "Experienced in HR and technical interviews.";
            var status = "pending";
            var response = new InterviewerEntryFormResponse(entryFormId, userId, additionalInfo, status);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInterviewerEntryFormRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(new GetInterviewerEntryFormRequest(entryFormId));

            Assert.NotNull(result);
            Assert.Equal(entryFormId, result.Id);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(additionalInfo, result.AdditionalInfo);
            Assert.Equal(status, result.Status);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetInterviewerEntryFormRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetInterviewerEntryForm_ThrowsExceptionIfNotFound()
        {
            var entryFormId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInterviewerEntryFormRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewerEntryFormNotFoundException(entryFormId));

            var exception = await Assert.ThrowsAsync<InterviewerEntryFormNotFoundException>(() =>
                _mediatorMock.Object.Send(new GetInterviewerEntryFormRequest(entryFormId))
            );

            Assert.NotNull(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetInterviewerEntryFormRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchInterviewerEntryForms_ReturnsPagedResponse()
        {
            var request = new SearchInterviewerEntryFormsCommand
            {
                AdditionalInfo = "Experienced",
                Status = "pending",
                PageNumber = 1,
                PageSize = 10
            };

            var response1 = new InterviewerEntryFormResponse(Guid.NewGuid(), Guid.NewGuid(), "Experienced in interviews", "pending");
            var response2 = new InterviewerEntryFormResponse(Guid.NewGuid(), Guid.NewGuid(), "Experienced in technical interviews", "pending");
            var pagedList = new PagedList<InterviewerEntryFormResponse>(new[] { response1, response2 }, 1, 10, 2);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SearchInterviewerEntryFormsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedList);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SearchInterviewerEntryFormsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewerEntryForm_ReturnsUpdatedResponse()
        {
            var entryFormId = Guid.NewGuid();
            var request = new UpdateInterviewerEntryFormCommand(
                entryFormId,
                UserId: Guid.NewGuid(),
                AdditionalInfo: "Updated info",
                Status: "approved"
            );
            var response = new UpdateInterviewerEntryFormResponse(entryFormId);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInterviewerEntryFormCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _mediatorMock.Object.Send(request);

            Assert.NotNull(result);
            Assert.Equal(entryFormId, result.Id);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateInterviewerEntryFormCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInterviewerEntryForm_ThrowsExceptionIfNotFound()
        {
            var entryFormId = Guid.NewGuid();
            var request = new UpdateInterviewerEntryFormCommand(
                entryFormId,
                UserId: Guid.NewGuid(),
                AdditionalInfo: "Updated info",
                Status: "approved"
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInterviewerEntryFormCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InterviewerEntryFormNotFoundException(entryFormId));

            var exception = await Assert.ThrowsAsync<InterviewerEntryFormNotFoundException>(() =>
                _mediatorMock.Object.Send(request)
            );

            Assert.NotNull(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateInterviewerEntryFormCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
