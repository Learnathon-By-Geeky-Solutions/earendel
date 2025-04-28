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

    public static class CreateInterviewerApplicationEndpoint
    {
        internal static RouteHandlerBuilder MapInterviewerApplicationCreationEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPost("/", async (CreateInterviewerApplicationCommand request, ISender mediator) =>
                {
                    var response = await mediator.Send(request);
                    return Results.Ok(response);
                })
                .WithName(nameof(CreateInterviewerApplicationEndpoint))
                .WithSummary("Creates an Interviewer Application")
                .WithDescription("Creates an Interviewer Application")
                .Produces<CreateInterviewerApplicationResponse>()
                .RequirePermission("Permissions.InterviewerApplications.Create")
                .MapToApiVersion(1);
        }
    }
}
