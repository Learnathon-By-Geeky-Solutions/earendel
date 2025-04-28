using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

    public static class CreateCandidateProfileEndpoint
    {
        internal static RouteHandlerBuilder MapCandidateProfileCreationEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPost("/", async (CreateCandidateProfileCommand request, ISender mediator) =>
                {
                    var response = await mediator.Send(request);
                    return Results.Ok(response);
                })
                .WithName(nameof(CreateCandidateProfileEndpoint))
                .WithSummary("Creates a Candidate Profile")
                .WithDescription("Creates a Candidate Profile")
                .Produces<CreateCandidateProfileResponse>()
                .RequirePermission("Permissions.CandidateProfiles.Create")
                .MapToApiVersion(1);
        }
    }
}
