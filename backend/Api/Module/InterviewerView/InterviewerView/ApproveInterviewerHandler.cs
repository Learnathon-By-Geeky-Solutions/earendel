using MediatR;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Application.Interviewer
{
    public class ApproveInterviewerHandler : IRequestHandler<ApproveInterviewerCommand, Unit>
    {
        private readonly IRepository<InterviewerEntryForm> _repo;
        public ApproveInterviewerHandler(IRepository<InterviewerEntryForm> repo)
            => _repo = repo;

        public async Task<Unit> Handle(ApproveInterviewerCommand req, CancellationToken ct)
        {
            var form = await _repo.GetByIdAsync(req.EntryFormId, ct)
                       ?? throw new KeyNotFoundException("Entry form not found.");

            // mark as approved
            form.Approve();

            // space for manual post‑approval steps (e.g. provisioning interviewer account)
            // — now remove the form from the DB:
            await _repo.DeleteAsync(form, ct);

            return Unit.Value;
        }
    }
}
