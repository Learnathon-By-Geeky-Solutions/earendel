using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Experties.Application.Rubrics.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TalentMesh.Module.Experties.Infrastructure.Endpoints.v1;
public static class CreateRubricEndpoint
{
    internal static RouteHandlerBuilder MapRubricCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateRubricCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateRubricEndpoint))
            .WithSummary("creates a Rubric")
            .WithDescription("creates a Rubric")
            .Produces<CreateRubricResponse>()
            // .RequirePermission("Permissions.Products.Create")
            .MapToApiVersion(1);
    }
}
