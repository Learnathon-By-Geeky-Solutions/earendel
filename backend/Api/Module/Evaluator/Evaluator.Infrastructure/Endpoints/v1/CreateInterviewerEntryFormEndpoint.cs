using TalentMesh.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

    public static class CreateInterviewerEntryFormEndpoint
    {
        internal static RouteHandlerBuilder MapInterviewerEntryFormCreationEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPost("/", async (CreateInterviewerEntryFormCommand request, ISender mediator) =>
                {
                    var response = await mediator.Send(request);
                    return Results.Ok(response);
                })
                .WithName(nameof(CreateInterviewerEntryFormEndpoint))
                .WithSummary("Creates Interviewer Entry Form")
                .WithDescription("Creates Interviewer Entry Form")
                .Produces<CreateInterviewerEntryFormResponse>()
                .RequirePermission("Permissions.Interviewer.Create")
                .MapToApiVersion(1);
        }
    }
}
