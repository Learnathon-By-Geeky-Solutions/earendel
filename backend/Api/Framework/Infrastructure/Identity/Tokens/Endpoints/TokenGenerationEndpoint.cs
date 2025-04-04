﻿using TalentMesh.Framework.Core.Identity.Tokens;
using TalentMesh.Framework.Core.Identity.Tokens.Features.Generate;
using TalentMesh.Shared.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Tokens.Endpoints;
[ExcludeFromCodeCoverage]

public static class TokenGenerationEndpoint
{
    internal static RouteHandlerBuilder MapTokenGenerationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", (TokenGenerationCommand request,
            // [FromHeader(Name = TenantConstants.Identifier)] string tenant,
            ITokenService service,
            HttpContext context,
            CancellationToken cancellationToken,
            [FromHeader(Name = TenantConstants.Identifier)] string? tenant = "root") =>
        {
            string ip = context.GetIpAddress();
            return service.GenerateTokenAsync(request, ip!, cancellationToken);
        })
        .WithName(nameof(TokenGenerationEndpoint))
        .WithSummary("generate JWTs")
        .WithDescription("generate JWTs")
        .AllowAnonymous();
    }
}
