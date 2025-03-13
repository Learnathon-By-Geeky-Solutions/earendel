using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Application.JobRequiredSkill.Get.v1;
using TalentMesh.Module.Job.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Search.v1
{
    public sealed class SearchJobRequiredSkillHandler(
        [FromKeyedServices("jobs:jobrequiredskillReadOnly")] IReadRepository<Domain.JobRequiredSkill> repository)
        : IRequestHandler<SearchJobRequiredSkillCommand, PagedList<JobRequiredSkillResponse>>
    {
        public async Task<PagedList<JobRequiredSkillResponse>> Handle(SearchJobRequiredSkillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var spec = new SearchJobRequiredSkillSpecs(request);
            var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
            var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);
            return new PagedList<JobRequiredSkillResponse>(items, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
