using TalentMesh.Framework.Infrastructure.Auth.Policy;

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Infrastructure.Endpoints.v1
{
    [ExcludeFromCodeCoverage]
    public static class UpdateInterviewerApplicationEndpoint
    {
        internal static RouteHandlerBuilder MapInterviewerApplicationUpdateEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPut("/{id:guid}", async (Guid id, UpdateInterviewerApplicationCommand request, ISender mediator) =>
                {
                    if (id != request.Id)
                        return Results.BadRequest();

                    var response = await mediator.Send(request);
                    return Results.Ok(response);
                })
                .WithName(nameof(UpdateInterviewerApplicationEndpoint))
                .WithSummary("Update an Interviewer Application")
                .WithDescription("Updates an Interviewer Application by its identifier")
                .Produces<UpdateInterviewerApplicationResponse>()
                 .RequirePermission("Permissions.Interviewer.Update")
                .MapToApiVersion(1);
        }
    }
}
