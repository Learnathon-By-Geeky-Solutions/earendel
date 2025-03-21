using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Skills.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class GetSkillEndpoint
{
    internal static RouteHandlerBuilder MapGetSkillEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetSkillRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetSkillEndpoint))
            .WithSummary("gets skill by id")
            .WithDescription("gets skill by id")
            .Produces<SkillResponse>()
            // .RequirePermission("Permissions.Jobs.View")
            .MapToApiVersion(1);
    }
}
