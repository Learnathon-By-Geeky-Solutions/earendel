using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Framework.Infrastructure.Messaging;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Framework.Core.Caching;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TalentMesh.Shared.Authorization; // for GetUserId() extension method

namespace TalentMesh.Module.Interviews.Application.Interviews.Get.v1
{
    public sealed class GetInterviewByInterviewerHandler(
        [FromKeyedServices("interviews:interviewReadOnly")] IReadRepository<Interview> repository,
        IMessageBus messageBus,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<GetInterviewByInterviewerRequest, bool>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<bool> Handle(GetInterviewByInterviewerRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            // Create a specification to fetch interviews for the specified interviewer.
            var spec = new InterviewByInterviewerIdSpec(request.Id);
            var interviews = await repository.ListAsync(spec, cancellationToken);

            // Get current userId from the HttpContext claims.
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.GetUserId(); // using extension method from TalentMesh.Shared.Authorization

            // Build the payload to be published.
            var interviewPayload = new
            {
                InterviewerId = request.Id,
                UserId = userId, // Include userId in the payload
                Interviews = interviews.Select(i => new
                {
                    i.Id,
                    i.ApplicationId,
                    i.InterviewDate,
                    i.Status,
                    i.Notes,
                    i.MeetingId
                })
            };

            await messageBus.PublishAsync(interviewPayload, "interview.application.events", "interview.application.getCandidate", cancellationToken);

            return true;
        }
    }
}
