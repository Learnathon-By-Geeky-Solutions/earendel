﻿using TalentMesh.Module.Experties.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Persistence.Configurations;
[ExcludeFromCodeCoverage]

internal sealed class SubSkillConfiguration : IEntityTypeConfiguration<Experties.Domain.SubSkill>
{
    public void Configure(EntityTypeBuilder<Experties.Domain.SubSkill> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(1000);
    }
}