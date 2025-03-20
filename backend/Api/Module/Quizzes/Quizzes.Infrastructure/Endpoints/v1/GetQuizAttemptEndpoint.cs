using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizAttempts.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class GetQuizAttemptEndpoint
{
    internal static RouteHandlerBuilder MapGetQuizAttemptEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetQuizAttemptRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetQuizAttemptEndpoint))
            .WithSummary("gets Quiz by id")
            .WithDescription("gets Quiz by id")
            .Produces<QuizAttemptResponse>()
            // .RequirePermission("Permissions.Jobs.View")
            .MapToApiVersion(1);
    }
}
