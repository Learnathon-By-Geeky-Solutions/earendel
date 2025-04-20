using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace TalentMesh.Framework.Infrastructure.Auth;
[ExcludeFromCodeCoverage]
public class CurrentUserMiddleware(ICurrentUserInitializer currentUserInitializer) : IMiddleware
{
    private readonly ICurrentUserInitializer _currentUserInitializer = currentUserInitializer;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.GetEndpoint();
        var isAnonymousAllowed = endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null;

        if (!isAnonymousAllowed && context.User?.Identity?.IsAuthenticated == true)
        {
            _currentUserInitializer.SetCurrentUser(context.User);
        }

        await next(context);
    }
}
