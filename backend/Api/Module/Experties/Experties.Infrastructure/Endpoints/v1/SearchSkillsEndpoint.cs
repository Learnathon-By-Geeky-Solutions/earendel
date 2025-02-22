using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Skills.Get.v1;
using TalentMesh.Module.Experties.Application.Skills.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;

public static class SearchSkillsEndpoint
{
    internal static RouteHandlerBuilder MapGetSkillListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchSkillsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchSkillsEndpoint))
            .WithSummary("Gets a list of skills")
            .WithDescription("Gets a list of skills with pagination and filtering support")
            .Produces<PagedList<SkillResponse>>()
            // .RequirePermission("Permissions.Products.View")
            .MapToApiVersion(1);
    }
}

