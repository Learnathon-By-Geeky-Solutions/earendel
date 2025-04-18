// Application/Interviewer/UploadCvCommand.cs
using MediatR;
using Microsoft.AspNetCore.Http;
using TalentMesh.Module.Evaluator.Domain;

namespace TalentMesh.Module.Evaluator.Application.Interviewer
{
    // Now returns the updated entity
    public record UploadCvCommand(Guid EntryFormId, IFormFile CvFile) : IRequest<InterviewerEntryForm>;
}
