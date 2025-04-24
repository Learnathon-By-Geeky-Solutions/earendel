using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TalentMesh.Module.Evaluator.Domain;
using TalentMesh.Module.Evaluator.Infrastructure.Persistence;

namespace TalentMesh.Module.InterviewerView
{

    // Response DTO for interviewer requests
    public record RequestInterviewerResult(Guid RequestId, string Status);

    public class RequestInterviewerHandler : IRequestHandler<RequestInterviewerCommand, RequestInterviewerResult>
    {
        private readonly EvaluatorDbContext _dbContext;

        public RequestInterviewerHandler(EvaluatorDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<RequestInterviewerResult> Handle(RequestInterviewerCommand req, CancellationToken cancellationToken)
        {
            // Prevent multiple pending requests by the same user
            var existing = await _dbContext.InterviewerEntryForms
                .FirstOrDefaultAsync(form => form.UserId == req.UserId && form.Status == "pending", cancellationToken);

            if (existing is not null)
            {
                return new RequestInterviewerResult(existing.Id, existing.Status);
            }

            // Create and save new interviewer entry form
            var form = InterviewerEntryForm.Create(req.UserId, req.AdditionalInfo);
            _dbContext.InterviewerEntryForms.Add(form);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new RequestInterviewerResult(form.Id, form.Status);
        }
    }
}
