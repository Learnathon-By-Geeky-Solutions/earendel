using MediatR;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Application.Interviewer
{
    public class RequestInterviewerHandler : IRequestHandler<RequestInterviewerCommand, Guid>
    {
        private readonly IRepository<InterviewerEntryForm> _repo;
        public RequestInterviewerHandler(IRepository<InterviewerEntryForm> repo) => _repo = repo;

        public async Task<Guid> Handle(RequestInterviewerCommand req, CancellationToken ct)
        {
            var form = InterviewerEntryForm.Create(req.UserId, req.AdditionalInfo);
            await _repo.AddAsync(form, ct);
            return form.Id;
        }
    }
}
