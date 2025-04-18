// Application/Interviewer/UploadCvHandler.cs
using MediatR;
using Microsoft.AspNetCore.Hosting;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Evaluator.Domain;
// using Carter.OpenApi; // Not needed here

namespace TalentMesh.Module.Evaluator.Application.Interviewer
{
    public class UploadCvHandler : IRequestHandler<UploadCvCommand, InterviewerEntryForm>
    {
        private readonly IRepository<InterviewerEntryForm> _repo;
        private readonly IWebHostEnvironment _env;

        public UploadCvHandler(IRepository<InterviewerEntryForm> repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public async Task<InterviewerEntryForm> Handle(UploadCvCommand req, CancellationToken ct)
        {
            var form = await _repo.GetByIdAsync(req.EntryFormId, ct)
                       ?? throw new KeyNotFoundException("Entry form not found.");

            // VVV REMOVE THIS CHECK - It's already performed in the endpoint VVV
            // if (req.CvFile.ContentType != "application/pdf")
            //     throw new InvalidDataException("Only PDF files are allowed.");
            // ^^^ REMOVE THIS CHECK ^^^

            var uploads = Path.Combine(_env.WebRootPath, "uploads", "cvs");
            Directory.CreateDirectory(uploads); // Ensures the directory exists

            // Consider making filename more robust/unique if needed beyond GUID
            var fileName = $"{req.EntryFormId}_{Guid.NewGuid()}.pdf";
            var filePath = Path.Combine(uploads, fileName);

            // Use FileStream with using statement for proper disposal
            await using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await req.CvFile.CopyToAsync(fs, ct);
            }

            // update domain entity - Ensure the path stored is consistent with how you retrieve it
            // Storing a relative path accessible via web requests is common.
            form.UploadCv($"/uploads/cvs/{fileName}");
            await _repo.UpdateAsync(form, ct);

            // return the updated entity
            return form;
        }
    }
}