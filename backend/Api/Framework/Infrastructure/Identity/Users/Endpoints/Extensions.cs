using TalentMesh.Framework.Infrastructure.Identity.Audit.Endpoints;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Endpoints;
internal static class Extensions
{
    [ExcludeFromCodeCoverage]

    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRegisterUserEndpoint();
        app.MapSelfRegisterUserEndpoint();
        app.MapGoogleLoginUserEndpoint();
        app.MapUpdateUserEndpoint();
        app.MapGetUsersListEndpoint();
        app.MapDeleteUserEndpoint();
        app.MapForgotPasswordEndpoint();
        app.MapChangePasswordEndpoint();
        app.MapResetPasswordEndpoint();
        app.MapGetMeEndpoint();
        app.MapGetUserEndpoint();
        app.MapGetCurrentUserPermissionsEndpoint();
        app.ToggleUserStatusEndpointEndpoint();
        app.MapAssignRolesToUserEndpoint();
        app.MapGetUserRolesEndpoint();
        app.MapGetUserAuditTrailEndpoint();
        return app;
    }
}
