using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class GetSeniorityEndpoint
{
    internal static RouteHandlerBuilder MapGetSeniorityEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetSeniorityRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetSeniorityEndpoint))
            .WithSummary("gets seniority by id")
            .WithDescription("gets seniority by id")
            .Produces<SeniorityResponse>()
            .RequirePermission("Permissions.Seniority.View")
            .MapToApiVersion(1);
    }
}
