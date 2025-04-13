using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using TalentMesh.Shared.Authorization;

namespace TalentMesh.Framework.Infrastructure.Auth;
[ExcludeFromCodeCoverage]
public class DefaultTenantMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        const string defaultTenant = "root";
        if (!context.Request.Headers.ContainsKey(TenantConstants.Identifier))
        {
            context.Request.Headers[TenantConstants.Identifier] = defaultTenant;

        }
        await next(context);
    }
}
