using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Application.Skills.Get.v1;
using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Experties.Application.Skills.Search.v1
{
    [ExcludeFromCodeCoverage]
    public sealed class SearchSkillsHandler(
        [FromKeyedServices("skills:skillReadOnly")] IReadRepository<Experties.Domain.Skill> repository)
        : IRequestHandler<SearchSkillsCommand, PagedList<SkillResponse>>
    {
        public async Task<PagedList<SkillResponse>> Handle(SearchSkillsCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var spec = new SearchSkillSpecs(request);

            var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
            var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

            return new PagedList<SkillResponse>(items, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
