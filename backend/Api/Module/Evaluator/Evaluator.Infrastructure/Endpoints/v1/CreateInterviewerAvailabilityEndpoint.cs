﻿using TalentMesh.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]

    public static class CreateInterviewerAvailabilityEndpoint
    {
        internal static RouteHandlerBuilder MapInterviewerAvailabilityCreationEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPost("/", async (CreateInterviewerAvailabilityCommand request, ISender mediator) =>
                {
                    var response = await mediator.Send(request);
                    return Results.Ok(response);
                })
                .WithName(nameof(CreateInterviewerAvailabilityEndpoint))
                .WithSummary("Creates Interviewer Availability")
                .WithDescription("Creates Interviewer Availability")
                .Produces<CreateInterviewerAvailabilityResponse>()
                .RequirePermission("Permissions.InterviewerApplications.Create")
                .MapToApiVersion(1);
        }
    }
}
