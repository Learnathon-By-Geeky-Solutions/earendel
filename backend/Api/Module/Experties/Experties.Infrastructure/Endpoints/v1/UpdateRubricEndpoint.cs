using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Rubrics.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class UpdateRubricEndpoint
{
    internal static RouteHandlerBuilder MapRubricUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateRubricCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateRubricEndpoint))
            .WithSummary("update a Rubric")
            .WithDescription("update a Rubric")
            .Produces<UpdateRubricResponse>()
            .RequirePermission("Permissions.Rubrics.Update")    // ← added
            .MapToApiVersion(1);
    }
}
