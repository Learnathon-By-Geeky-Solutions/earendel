using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Candidate.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

    public static class DeleteCandidateProfileEndpoint
    {
        internal static RouteHandlerBuilder MapCandidateProfileDeleteEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    await mediator.Send(new DeleteCandidateProfileCommand(id));
                    return Results.NoContent();
                })
                .WithName(nameof(DeleteCandidateProfileEndpoint))
                .WithSummary("Deletes a Candidate Profile by id")
                .WithDescription("Deletes a Candidate Profile by id")
                .Produces(StatusCodes.Status204NoContent)
                .RequirePermission("Permissions.CandidateProfiles.Delete")
                .MapToApiVersion(1);
        }
    }
}
