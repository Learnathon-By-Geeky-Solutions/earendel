﻿using TalentMesh.Framework.Core.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Storage.Files;

[ExcludeFromCodeCoverage]

internal static class Extension
{
    internal static IServiceCollection ConfigureFileStorage(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddTransient<IStorageService, LocalFileStorageService>();

        return services;
    }

    internal static IApplicationBuilder UseFileStorage(this IApplicationBuilder app) =>
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
            RequestPath = new PathString("/Files")
        });
}
