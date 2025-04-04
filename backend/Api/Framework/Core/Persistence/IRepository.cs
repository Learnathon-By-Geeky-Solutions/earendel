﻿using Ardalis.Specification;
using TalentMesh.Framework.Core.Domain.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Persistence;
public interface IRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot
{
}

public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class, IAggregateRoot
{
}
