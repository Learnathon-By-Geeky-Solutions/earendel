
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Module.User.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace TalentMesh.Module.User.Infrastructure.Persistence;

public sealed class UserDbContext : DbContext
{
    // Constructor now only requires the EF Core DbContextOptions.
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    // DbSet properties for your domain entities.
    public DbSet<User> Products { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        // Apply all configurations from the current assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);

        // Set the default schema. You can either keep the constant or use a literal.
        modelBuilder.HasDefaultSchema("Catalog");
    }
}
