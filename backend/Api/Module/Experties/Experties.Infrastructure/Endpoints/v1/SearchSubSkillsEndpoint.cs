using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using TalentMesh.Module.Experties.Application.SubSkills.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class SearchSubSkillsEndpoint
{
    internal static RouteHandlerBuilder MapGetSubSkillListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchSubSkillsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchSubSkillsEndpoint))
            .WithSummary("Gets a list of subskills")
            .WithDescription("Gets a list of subskills with pagination and filtering support")
            .Produces<PagedList<SubSkillResponse>>()
            .RequirePermission("Permissions.SubSkills.View")    // ← added
            .MapToApiVersion(1);
    }
}

