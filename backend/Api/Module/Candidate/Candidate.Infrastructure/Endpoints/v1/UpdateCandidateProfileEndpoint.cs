using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

    public static class UpdateCandidateProfileEndpoint
    {
        internal static RouteHandlerBuilder MapCandidateProfileUpdateEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPut("/{id:guid}", async (Guid id, UpdateCandidateProfileCommand request, ISender mediator) =>
                {
                    if (id != request.Id)
                        return Results.BadRequest();

                    var response = await mediator.Send(request);
                    return Results.Ok(response);
                })
                .WithName(nameof(UpdateCandidateProfileEndpoint))
                .WithSummary("Updates a candidate profile")
                .WithDescription("Updates a candidate profile by its identifier")
                .Produces<UpdateCandidateProfileResponse>()
                .RequirePermission("Permissions.CandidateProfiles.Update")
                .MapToApiVersion(1);
        }
    }
}
