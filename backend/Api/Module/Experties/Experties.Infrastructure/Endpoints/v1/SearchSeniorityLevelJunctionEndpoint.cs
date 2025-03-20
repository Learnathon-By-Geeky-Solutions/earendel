using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class SearchSeniorityLevelJunctionEndpoint
{
    internal static RouteHandlerBuilder MapGetSeniorityLevelJunctionListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchSeniorityLevelJunctionCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchSeniorityLevelJunctionEndpoint))
            .WithSummary("Gets a list of seniority level junctions")
            .WithDescription("Gets a list of seniority level junctions with pagination and filtering support")
            .Produces<PagedList<SeniorityLevelJunctionResponse>>()
            // .RequirePermission("Permissions.SeniorityLevelJunctions.View")
            .MapToApiVersion(1);
    }
}