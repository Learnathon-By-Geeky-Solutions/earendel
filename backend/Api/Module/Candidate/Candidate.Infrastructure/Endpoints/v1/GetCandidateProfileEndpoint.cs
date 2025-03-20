using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

    public static class GetCandidateProfileEndpoint
    {
        internal static RouteHandlerBuilder MapGetCandidateProfileEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    var response = await mediator.Send(new GetCandidateProfileRequest(id));
                    return Results.Ok(response);
                })
                .WithName(nameof(GetCandidateProfileEndpoint))
                .WithSummary("Gets a candidate profile by id")
                .WithDescription("Gets a candidate profile by id")
                .Produces<CandidateProfileResponse>()
                .RequirePermission("Permissions.CandidateProfiles.View")
                .MapToApiVersion(1);
        }
    }
}
