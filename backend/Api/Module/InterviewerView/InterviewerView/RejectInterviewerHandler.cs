using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Framework.Infrastructure.Identity.Users;
using TalentMesh.Framework.Infrastructure.Identity.Users.Services;
using TalentMesh.Framework.Infrastructure.Identity.Roles;
using TalentMesh.Module.Evaluator.Domain;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Shared.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.InterviewerView
{
    [ExcludeFromCodeCoverage]
    public class RejectInterviewerHandler : IRequestHandler<RejectInterviewerCommand, Unit>
    {
        private readonly IRepository<InterviewerEntryForm> _repo;
        private readonly UserManager<TMUser> _userManager;

        public RejectInterviewerHandler(
            IRepository<InterviewerEntryForm> repo,
            IdentityServices identityServices)
        {
            _repo = repo;
            _userManager = identityServices.UserManager;
        }

        public async Task<Unit> Handle(RejectInterviewerCommand req, CancellationToken cancellationToken)
        {
            var form = await _repo.GetByIdAsync(req.EntryFormId, cancellationToken)
                       ?? throw new KeyNotFoundException("Entry form not found.");

            // Mark as rejected
            form.Reject();

            // Assign Interviewer role to the user associated with this form
            // Adjust property name/type if necessary
            var userId = form.UserId.ToString();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            // Remove the form from the DB
            await _repo.DeleteAsync(form, cancellationToken);

            return Unit.Value;
        }
    }
}
