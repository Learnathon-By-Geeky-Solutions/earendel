using MediatR;
using Microsoft.AspNetCore.Http; // For IResult
using System;

namespace TalentMesh.Module.HRView.HRFunc
{
    // Query includes the User ID for filtering
    public record GetMyJobsQuery(
        Guid RequestingUserId, // ID of the user viewing their posted jobs
        int PageNumber = 1,
        int PageSize = 20
    ) : IRequest<IResult>;
}