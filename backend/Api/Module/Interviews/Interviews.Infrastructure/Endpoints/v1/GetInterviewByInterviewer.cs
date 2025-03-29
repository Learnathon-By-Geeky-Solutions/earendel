using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.Interviews.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class GetInterviewByInterviewerEndpoint
{
    internal static RouteHandlerBuilder MapGetInterviewByInterviewerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/interviewer/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInterviewByInterviewerRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetInterviewByInterviewerEndpoint))
            .WithSummary("gets Interview by interviewer id")
            .WithDescription("gets Interview by interviewer id")
            .Produces<bool>()
            // .RequirePermission("Permissions.Jobs.View")
            .MapToApiVersion(1);
    }
}
