using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Skills.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class CreateSkillEndpoint
{
    internal static RouteHandlerBuilder MapSkillCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateSkillCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateSkillEndpoint))
            .WithSummary("creates a skill")
            .WithDescription("creates a skill")
            .Produces<CreateSkillResponse>()
            .RequirePermission("Permissions.Skills.Create")    // ← added
            .MapToApiVersion(1);
    }
}
