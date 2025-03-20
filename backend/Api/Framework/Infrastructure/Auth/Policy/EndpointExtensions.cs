using Microsoft.AspNetCore.Builder;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Auth.Policy;
[ExcludeFromCodeCoverage]
public static class EndpointExtensions
{
    public static TBuilder RequirePermission<TBuilder>(
    this TBuilder endpointConventionBuilder, string requiredPermission, params string[] additionalRequiredPermissions)
    where TBuilder : IEndpointConventionBuilder
    {
        return endpointConventionBuilder.WithMetadata(new RequiredPermissionAttribute(requiredPermission, additionalRequiredPermissions));
    }
}
