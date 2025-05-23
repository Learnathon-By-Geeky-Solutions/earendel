﻿using TalentMesh.Framework.Infrastructure.Auth.Policy;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Endpoints.v1;
[ExcludeFromCodeCoverage]

public static class DeleteInterviewFeedbackEndpoint
{
    internal static RouteHandlerBuilder MapInterviewFeedbackDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteInterviewFeedbackCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteInterviewFeedbackEndpoint))
            .WithSummary("deletes answer by id")
            .WithDescription("deletes answer by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.InterviewFeedbacks.Delete") // ✅ Corrected permission
            .MapToApiVersion(1);
    }
}
