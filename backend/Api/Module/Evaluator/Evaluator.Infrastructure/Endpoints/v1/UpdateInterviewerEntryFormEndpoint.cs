using TalentMesh.Framework.Infrastructure.Auth.Policy;

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1;

namespace TalentMesh.Module.Evaluator.Infrastructure.Endpoints.v1
{
    public static class UpdateInterviewerEntryFormEndpoint
    {
        internal static RouteHandlerBuilder MapInterviewerEntryFormUpdateEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapPut("/{id:guid}", async (Guid id, UpdateInterviewerEntryFormCommand request, ISender mediator) =>
                {
                    if (id != request.Id)
                        return Results.BadRequest();

                    var response = await mediator.Send(request);
                    return Results.Ok(response);
                })
                .WithName(nameof(UpdateInterviewerEntryFormEndpoint))
                .WithSummary("Update Interviewer Entry Form")
                .WithDescription("Updates Interviewer Entry Form by its identifier")
                .Produces<UpdateInterviewerEntryFormResponse>()
                 .RequirePermission("Permissions.Interviewer.Update")
                .MapToApiVersion(1);
        }
    }
}
