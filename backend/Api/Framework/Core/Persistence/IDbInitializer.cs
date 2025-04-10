﻿using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Persistence;
public interface IDbInitializer
{
    Task MigrateAsync(CancellationToken cancellationToken);
    Task SeedAsync(CancellationToken cancellationToken);
}
