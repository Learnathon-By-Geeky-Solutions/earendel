using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace TalentMesh.Module.CandidateLogic.JobApplicationView // Or your preferred namespace
{
    public record JobApplicationViewFilters(
        Guid? JobId,
        Guid? CandidateId, // User ID
        DateTime? ApplicationDateStart,
        DateTime? ApplicationDateEnd,
        string? Status
        // Add other potential filters like pagination parameters if needed
        ) : IRequest<IResult>;
}