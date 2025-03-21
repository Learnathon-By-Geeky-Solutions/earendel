using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Get.v1;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

    public static class SearchCandidateProfilesEndpoint
    {
        internal static RouteHandlerBuilder MapGetCandidateProfileListEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPost("/search", async (ISender mediator, [FromBody] SearchCandidateProfileCommand command) =>
                {
                    var response = await mediator.Send(command);
                    return Results.Ok(response);
                })
                .WithName(nameof(SearchCandidateProfilesEndpoint))
                .WithSummary("Gets a list of candidate profiles")
                .WithDescription("Gets a list of candidate profiles with pagination and filtering support")
                .Produces<PagedList<CandidateProfileResponse>>()
                .RequirePermission("Permissions.CandidateProfiles.View")
                .MapToApiVersion(1);
        }
    }
}
