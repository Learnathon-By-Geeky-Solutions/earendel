using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Framework.Core.Persistence;
using Mapster;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Notifications.Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
internal sealed class NotificationsRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public NotificationsRepository(NotificationsDbContext context)
        : base(context)
    {
    }

    // We override the default behavior when mapping to a dto.
    // We're using Mapster's ProjectToType here to immediately map the result from the database.
    // This is only done when no Selector is defined, so regular specifications with a selector also still work.
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        specification.Selector is not null
            ? base.ApplySpecification(specification)
            : ApplySpecification(specification, false)
                .ProjectToType<TResult>();
}
