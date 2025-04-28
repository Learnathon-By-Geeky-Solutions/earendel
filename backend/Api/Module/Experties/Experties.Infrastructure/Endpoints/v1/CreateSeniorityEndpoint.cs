using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Seniorities.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class CreateSeniorityEndpoint
{
    internal static RouteHandlerBuilder MapSeniorityCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateSeniorityCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateSeniorityEndpoint))
            .WithSummary("creates a seniority")
            .WithDescription("creates a seniority")
            .Produces<CreateSeniorityResponse>()
            .RequirePermission("Permissions.Seniorities.Create")    // ← added
            .MapToApiVersion(1);
    }
}
