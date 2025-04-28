using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.Interviews.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class CreateInterviewEndpoint
{
    internal static RouteHandlerBuilder MapInterviewCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateInterviewCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateInterviewEndpoint))
            .WithSummary("Interview")
            .WithDescription("Interview")
            .Produces<CreateInterviewResponse>()
            .RequirePermission("Permissions.Interviews.Create")  
            .MapToApiVersion(1);
    }
}
