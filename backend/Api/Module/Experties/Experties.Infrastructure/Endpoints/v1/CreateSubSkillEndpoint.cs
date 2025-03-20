using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.SubSkills.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class CreateSubSkillEndpoint
{
    internal static RouteHandlerBuilder MapSubSkillCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateSubSkillCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateSubSkillEndpoint))
            .WithSummary("creates a subskill")
            .WithDescription("creates a subskill")
            .Produces<CreateSubSkillResponse>()
            // .RequirePermission("Permissions.Products.Create")
            .MapToApiVersion(1);
    }
}
