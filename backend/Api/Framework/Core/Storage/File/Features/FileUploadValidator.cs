﻿using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Storage.File.Features;
[ExcludeFromCodeCoverage]
public class FileUploadRequestValidator : AbstractValidator<FileUploadCommand>
{
    public FileUploadRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(p => p.Extension)
            .NotEmpty()
            .MaximumLength(5);

        RuleFor(p => p.Data)
            .NotEmpty();  
    }
}

