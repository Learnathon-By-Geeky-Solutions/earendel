using TalentMesh.Framework.Core.Tenant.Features.GetTenantById;
using TalentMesh.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Tenant.Endpoints;
[ExcludeFromCodeCoverage]

public static class GetTenantByIdEndpoint
{
    internal static RouteHandlerBuilder MapGetTenantByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id}", (ISender mediator, string id) => mediator.Send(new GetTenantByIdQuery(id)))
                                .WithName(nameof(GetTenantByIdEndpoint))
                                .WithSummary("get tenant by id")
                                .RequirePermission("Permissions.Tenants.View")
                                .WithDescription("get tenant by id");
    }
}
