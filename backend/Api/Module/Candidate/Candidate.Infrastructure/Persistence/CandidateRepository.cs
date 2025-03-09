using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Framework.Core.Persistence;
using Mapster;
using TalentMesh.Module.Candidate.Infrastructure.Persistence;

namespace TalentMesh.Module.Candidate.Infrastructure.Persistence
{
    internal sealed class CandidateRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
         where T : class, IAggregateRoot
    {
        public CandidateRepository(CandidateDbContext context)
            : base(context)
        {
        }

        // Override the default behavior when mapping to a DTO.
        // If no Selector is defined, we use Mapster's ProjectToType to map the result immediately.
        protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
            specification.Selector is not null
                ? base.ApplySpecification(specification)
                : ApplySpecification(specification, false)
                    .ProjectToType<TResult>();
    }
}
