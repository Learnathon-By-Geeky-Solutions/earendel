﻿using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class CreateInterviewQuestionEndpoint
{
    internal static RouteHandlerBuilder MapInterviewQuestionCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateInterviewQuestionCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateInterviewQuestionEndpoint))
            .WithSummary("InterviewQuestion")
            .WithDescription("Interview Question")
            .Produces<CreateInterviewQuestionResponse>()
            .RequirePermission("Permissions.InterviewQuestions.Create") // ✅ Corrected permission
            .MapToApiVersion(1);
    }
}
