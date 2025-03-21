using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Auth;
[ExcludeFromCodeCoverage]
public class CurrentUserMiddleware(ICurrentUserInitializer currentUserInitializer) : IMiddleware
{
    private readonly ICurrentUserInitializer _currentUserInitializer = currentUserInitializer;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _currentUserInitializer.SetCurrentUser(context.User);
        await next(context);
    }
}
