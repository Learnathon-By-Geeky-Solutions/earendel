using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class GetSubSkillEndpoint
{
    internal static RouteHandlerBuilder MapGetSubSkillEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetSubSkillRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetSubSkillEndpoint))
            .WithSummary("gets subskill by id")
            .WithDescription("gets subskill by id")
            .Produces<SubSkillResponse>()
            .RequirePermission("Permissions.SubSkills.View")    // ← added
            .MapToApiVersion(1);
    }
}
