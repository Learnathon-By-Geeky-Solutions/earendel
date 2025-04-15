using MediatR;
using Microsoft.AspNetCore.Http; // For IResult
using System;

namespace TalentMesh.Module.HRView.HRFunc // Or your preferred namespace
{
    // Query to get all job applications, potentially with pagination
    public record GetAllJobApplicationsQuery(
        int PageNumber = 1, // Default to page 1
        int PageSize = 20   // Default page size
    ) : IRequest<IResult>; // Returns a list (or paginated list) of JobApplicationDto
}