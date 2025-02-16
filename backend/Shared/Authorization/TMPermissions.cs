using System.Collections.ObjectModel;

namespace TalentMesh.Shared.Authorization;

public static class TMPermissions
{
    private static readonly FshPermission[] AllPermissions =
    [     
        //tenants
        new("View Tenants", TMActions.View, TMResources.Tenants, IsRoot: true),
        new("Create Tenants", TMActions.Create, TMResources.Tenants, IsRoot: true),
        new("Update Tenants", TMActions.Update, TMResources.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", TMActions.UpgradeSubscription, TMResources.Tenants, IsRoot: true),

        //identity
        new("View Users", TMActions.View, TMResources.Users),
        new("Search Users", TMActions.Search, TMResources.Users),
        new("Create Users", TMActions.Create, TMResources.Users),
        new("Update Users", TMActions.Update, TMResources.Users),
        new("Delete Users", TMActions.Delete, TMResources.Users),
        new("Export Users", TMActions.Export, TMResources.Users),
        new("View UserRoles", TMActions.View, TMResources.UserRoles),
        new("Update UserRoles", TMActions.Update, TMResources.UserRoles),
        new("View Roles", TMActions.View, TMResources.Roles),
        new("Create Roles", TMActions.Create, TMResources.Roles),
        new("Update Roles", TMActions.Update, TMResources.Roles),
        new("Delete Roles", TMActions.Delete, TMResources.Roles),
        new("View RoleClaims",  TMActions.View, TMResources.RoleClaims),
        new("Update RoleClaims", TMActions.Update, TMResources.RoleClaims),
        
        //Jobs
        new("View Products", TMActions.View, TMResources.Jobs, IsBasic: true),
        new("Search Products", TMActions.Search, TMResources.Jobs, IsBasic: true),
        new("Create Products", TMActions.Create, TMResources.Jobs),
        new("Update Products", TMActions.Update, TMResources.Jobs),
        new("Delete Products", TMActions.Delete, TMResources.Jobs),
        new("Export Products", TMActions.Export, TMResources.Jobs),



         new("View Hangfire", TMActions.View, TMResources.Hangfire),
         new("View Dashboard", TMActions.View, TMResources.Dashboard),

        //audit
        new("View Audit Trails", TMActions.View, TMResources.AuditTrails),
    ];

    public static IReadOnlyList<FshPermission> All { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions);
    public static IReadOnlyList<FshPermission> Root { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Admin { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Basic { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => p.IsBasic).ToArray());
}

public record FshPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource)
    {
        return $"Permissions.{resource}.{action}";
    }
}


