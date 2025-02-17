using FluentValidation;
using FluentValidation.Results;
using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Core.Identity.Users.Features.ChangePassword;
using TalentMesh.Framework.Core.Origin;
using TalentMesh.Shared.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;
public static class ChangePasswordEndpoint
{
    internal static RouteHandlerBuilder MapChangePasswordEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/change-password", async (ChangePasswordCommand command,
            HttpContext context,
            IOptions<OriginOptions> settings,
            IValidator<ChangePasswordCommand> validator,
            IUserService userService,
            CancellationToken cancellationToken) =>
        {
            ValidationResult result = await validator.ValidateAsync(command, cancellationToken);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }

            if (context.User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
            {
                return Results.BadRequest();
            }

            await userService.ChangePasswordAsync(command, userId);
            return Results.Ok("password reset email sent");
        })
        .WithName(nameof(ChangePasswordEndpoint))
        .WithSummary("Changes password")
        .WithDescription("Change password");
    }

}
