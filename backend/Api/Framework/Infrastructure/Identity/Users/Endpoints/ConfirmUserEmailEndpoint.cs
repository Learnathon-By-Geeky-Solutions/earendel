using FluentValidation;
using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Core.Origin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics.CodeAnalysis;
using TalentMesh.Shared.Authorization;
using Microsoft.AspNetCore.Http;


namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;

[ExcludeFromCodeCoverage]
public static class ConfirmUserEmailEndpoint
{
    internal static RouteHandlerBuilder MapConfirmUserEmailEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/confirm-email", async (
            [FromQuery] string userId,
            [FromQuery] string code,
            [FromQuery] string tenacy,
            IUserService service,
            HttpContext context,
            CancellationToken cancellationToken
            ) =>
        {

            var result = await service.ConfirmEmailAsync(userId, code, tenacy, cancellationToken);
            return Results.Ok(result);
        })
        .AllowAnonymous()
        .WithName(nameof(ConfirmUserEmailEndpoint))
        .WithSummary("Confirm user email")
        .WithDescription("Validates an email confirmation token to confirm a user's email address");
    }
}