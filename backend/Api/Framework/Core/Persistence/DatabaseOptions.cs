﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Persistence;
[ExcludeFromCodeCoverage]
public class DatabaseOptions : IValidatableObject
{
    public string Provider { get; set; } = "postgresql";
    public string ConnectionString { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(ConnectionString))
        {
            yield return new ValidationResult("connection string cannot be empty.", new[] { nameof(ConnectionString) });
        }
    }
}
