using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.Interviews.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class DeleteInterviewEndpoint
{
    internal static RouteHandlerBuilder MapInterviewDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteInterviewCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteInterviewEndpoint))
            .WithSummary("deletes answer by id")
            .WithDescription("deletes answer by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Interviews.Delete") // ✅ Corrected permission
            .MapToApiVersion(1);
    }
}
