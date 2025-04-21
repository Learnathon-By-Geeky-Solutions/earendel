using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Framework.Core.Persistence;
using Mapster;
using System.Diagnostics.CodeAnalysis;
using TalentMesh.Framework.Core.Specifications;

namespace TalentMesh.Module.Experties.Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
internal sealed class ExpertiesRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public ExpertiesRepository(ExpertiesDbContext context)
        : base(context)
    {
    }

    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        return specification.HasSelector()
            ? base.ApplySpecification(specification)
            : ApplySpecification(specification, false).ProjectToType<TResult>();
    }
}

