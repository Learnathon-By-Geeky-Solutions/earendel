using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Framework.Core.Persistence;
using Mapster;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
internal sealed class EvaluatorRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public EvaluatorRepository(EvaluatorDbContext context)
        : base(context)
    {
    }

    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        specification.Selector is not null
            ? base.ApplySpecification(specification)
            : ApplySpecification(specification, false)
                .ProjectToType<TResult>();
}
