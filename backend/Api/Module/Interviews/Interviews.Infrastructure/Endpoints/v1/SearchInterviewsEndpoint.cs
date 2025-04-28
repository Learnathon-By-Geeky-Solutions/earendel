using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.Interviews.Get.v1;
using TalentMesh.Module.Interviews.Application.Interviews.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class SearchInterviewsEndpoint
{
    internal static RouteHandlerBuilder MapGetInterviewListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchInterviewsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInterviewsEndpoint))
            .WithSummary("Gets a list of Interview")
            .WithDescription("Gets a list of Interview with pagination and filtering support")
            .Produces<PagedList<InterviewResponse>>()
            .RequirePermission("Permissions.Interviews.View")
            .MapToApiVersion(1);
    }
}

