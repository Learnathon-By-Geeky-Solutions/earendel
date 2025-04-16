using MediatR;
using Microsoft.AspNetCore.Http; // For IResult
using System;

namespace TalentMesh.Module.HRView.HRFunc // Or your preferred namespace
{
    // Query now includes the User ID for filtering
    public record GetAllJobApplicationsQuery(
        Guid RequestingUserId, // ID of the user viewing their job applications
        int PageNumber = 1,
        int PageSize = 20
    ) : IRequest<IResult>;
}