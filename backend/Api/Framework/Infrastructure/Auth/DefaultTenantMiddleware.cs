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
            // Note: Although you can add headers to the response easily,
            // adding them to the request is not always supported.
            // Instead, store the tenant in HttpContext.Items for later use.
            Console.WriteLine("set headers value");
            // context.Items[TenantConstants.Identifier] = defaultTenant;
            context.Request.Headers[TenantConstants.Identifier] = "root";

        }
        await next(context);
    }
}
