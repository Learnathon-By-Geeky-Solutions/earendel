using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using TalentMesh.Module.Experties.Application.Seniorities.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;

public static class SearchSenioritiesEndpoint
{
    internal static RouteHandlerBuilder MapGetSeniorityListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchSenioritiesCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchSenioritiesEndpoint))
            .WithSummary("Gets a list of seniorities")
            .WithDescription("Gets a list of seniorities with pagination and filtering support")
            .Produces<PagedList<SeniorityResponse>>()
            // .RequirePermission("Permissions.Products.View")
            .MapToApiVersion(1);
    }
}

