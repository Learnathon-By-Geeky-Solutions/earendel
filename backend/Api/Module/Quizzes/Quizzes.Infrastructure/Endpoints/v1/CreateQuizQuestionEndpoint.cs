﻿using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Quizzes.Application.QuizQuestions.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class CreateQuizQuestionEndpoint
{
    internal static RouteHandlerBuilder MapQuizQuestionCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateQuizQuestionCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateQuizQuestionEndpoint))
            .WithSummary("creates a Question")
            .WithDescription("creates a Question")
            .Produces<CreateQuizQuestionResponse>()
            .RequirePermission("Permissions.QuizQuestions.Create")    // ← enforce Create permission
            .MapToApiVersion(1);
    }
}
