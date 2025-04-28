using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.SubSkills.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class UpdateSubSkillEndpoint
{
    internal static RouteHandlerBuilder MapSubSkillUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateSubSkillCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateSubSkillEndpoint))
            .WithSummary("update a subskill")
            .WithDescription("update a subskill")
            .Produces<UpdateSubSkillResponse>()
            .RequirePermission("Permissions.SubSkills.Update")    // ← added
            .MapToApiVersion(1);
    }
}
