using Carter;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Persistence;
using TalentMesh.Module.Notifications.Domain;
using TalentMesh.Module.Notifications.Infrastructure.Endpoints.v1;
using TalentMesh.Module.Notifications.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Notifications.Infrastructure;
[ExcludeFromCodeCoverage]

public static class NotificationsModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("notifications") { }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var notificationGroup = app.MapGroup("notifications").WithTags("notifications");
            notificationGroup.MapNotificationCreationEndpoint();
            notificationGroup.MapGetNotificationEndpoint();
            notificationGroup.MapGetNotificationListEndpoint();
            notificationGroup.MapNotificationUpdateEndpoint();
            notificationGroup.MapNotificationDeleteEndpoint();


        }
    }
    public static WebApplicationBuilder RegisterNotificationsServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<NotificationsDbContext>();
        builder.Services.AddScoped<IDbInitializer, NotificationsDbInitializer>();

        builder.Services.AddKeyedScoped<IRepository<Notification>, NotificationsRepository<Notification>>("notifications:notification");
        builder.Services.AddKeyedScoped<IReadRepository<Notification>, NotificationsRepository<Notification>>("notifications:notificationReadOnly");

        return builder;
    }
    public static WebApplication UseNotificationsModule(this WebApplication app)
    {
        return app;
    }
}
