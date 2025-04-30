using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.Interviews.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class CreateInterviewSignatureEndpoint
{
    internal static RouteHandlerBuilder MapInterviewSignatureCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/signature", async (CreateInterviewSignatureCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateInterviewSignatureEndpoint))
            .WithSummary("Interview Signature")
            .WithDescription("Interview Signature")
            .Produces<CreateInterviewSignatureResponse>()
            .RequirePermission("Permissions.InterviewSignatures.View") // ✅ Corrected permission
            .MapToApiVersion(1);
    }
}
