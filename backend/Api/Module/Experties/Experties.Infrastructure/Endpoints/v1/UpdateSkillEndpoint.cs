using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Skills.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class UpdateSkillEndpoint
{
    internal static RouteHandlerBuilder MapSkillUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateSkillCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateSkillEndpoint))
            .WithSummary("update a skill")
            .WithDescription("update a skill")
            .Produces<UpdateSkillResponse>()
            .RequirePermission("Permissions.Skills.Update")    // ← added
            .MapToApiVersion(1);
    }
}
