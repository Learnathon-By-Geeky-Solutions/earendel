using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Rubrics.Get.v1;
using TalentMesh.Module.Experties.Application.Rubrics.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class SearchRubricsEndpoint
{
    internal static RouteHandlerBuilder MapGetRubricListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchRubricsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchRubricsEndpoint))
            .WithSummary("Gets a list of Rubrics")
            .WithDescription("Gets a list of Rubrics with pagination and filtering support")
            .Produces<PagedList<RubricResponse>>()
            .RequirePermission("Permissions.Rubrics.View")    // ← added
            .MapToApiVersion(1);
    }
}

