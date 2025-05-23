﻿using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Evaluator.Domain;
using Asp.Versioning.Conventions;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Endpoints
{
    [ExcludeFromCodeCoverage]
    public static class InterviewerEndpoints
    {
        public static WebApplication MapInterviewerEndpoints(this WebApplication app)
        {
            // build a version‐set once
            var versionSet = app.NewApiVersionSet()
                                .HasApiVersion(1.0)
                                .ReportApiVersions()
                                .Build();

            // map the POST and attach the version metadata
            app.MapPost(
                    "/api/v1/interviewers/{id:guid}/upload-cv",
                    HandleCvUploadAsync
                )
               .WithApiVersionSet(versionSet)      // <- tie in the version set
               .MapToApiVersion(1.0)
               .DisableAntiforgery()    // <- declare this endpoint is v1.0
               .WithName("UploadInterviewerCv")
               .WithTags("InterviewerFileHandler")
               .RequirePermission("Permissions.InterviewerEntryForms.Create")
               .Produces<InterviewerEntryForm>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .ProducesProblem(StatusCodes.Status404NotFound)
               .ProducesProblem(StatusCodes.Status500InternalServerError);



            app.MapGet(
                    "/api/v1/interviewers/{id:guid}/download-cv",
                    HandleCvDownloadAsync
                )
               .WithApiVersionSet(versionSet)
               .MapToApiVersion(1.0)
               .WithName("DownloadInterviewerCv")
               .WithTags("InterviewerFileHandler")
                .RequirePermission("Permissions.InterviewerEntryForms.View")
               .Produces(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status404NotFound);



            return app;
        }

        private static async Task<IResult> HandleCvUploadAsync(
            Guid id,
            IFormFile cv,
            IWebHostEnvironment env,
            IRepository<InterviewerEntryForm> repo,
            CancellationToken ct)
        {
            if (cv is null || cv.Length == 0)
                return Results.BadRequest("CV is required.");

            if (cv.ContentType != "application/pdf")
                return Results.BadRequest("Only PDF files are allowed.");

            var form = await repo.GetByIdAsync(id, ct);
            if (form is null)
                return Results.NotFound();

            var uploadDir = Path.Combine(env.WebRootPath ?? "", "uploads", "cvs");
            Directory.CreateDirectory(uploadDir);

            var fileName = $"{id}_{Guid.NewGuid()}.pdf";
            var fullPath = Path.Combine(uploadDir, fileName);
            await using var fs = new FileStream(fullPath, FileMode.Create);
            await cv.CopyToAsync(fs, ct);

            form.UploadCv($"/uploads/cvs/{fileName}");
            await repo.UpdateAsync(form, ct);

            return Results.Ok(form);
        }
        private static async Task<IResult> HandleCvDownloadAsync(
            Guid id,
            IWebHostEnvironment env,
            IRepository<InterviewerEntryForm> repo,
            CancellationToken ct)
        {
            var form = await repo.GetByIdAsync(id, ct);
            if (form is null || string.IsNullOrEmpty(form.CV))
                return Results.NotFound();

            // Convert the stored relative path to physical path
            var relative = form.CV.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var filePath = Path.Combine(env.WebRootPath ?? string.Empty, relative);
            if (!File.Exists(filePath))
                return Results.NotFound();

            var fileName = Path.GetFileName(filePath);
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Results.File(stream, "application/pdf", fileName);
        }
    }
}
