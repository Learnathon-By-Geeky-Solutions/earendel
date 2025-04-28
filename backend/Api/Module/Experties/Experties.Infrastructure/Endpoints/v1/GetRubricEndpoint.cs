using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Rubrics.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class GetRubricEndpoint
{
    internal static RouteHandlerBuilder MapGetRubricEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetRubricRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetRubricEndpoint))
            .WithSummary("gets Rubric by id")
            .WithDescription("gets Rubric by id")
            .Produces<RubricResponse>()
            .RequirePermission("Permissions.Rubrics.View")
            .MapToApiVersion(1);
    }
}
