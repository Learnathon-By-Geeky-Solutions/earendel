using System.Collections.ObjectModel;
using TalentMesh.Shared.Authorization;

namespace TalentMesh.Shared.Authorization;

public static class TMPermissions
{
    private static readonly FshPermission[] AllPermissions =
    {
        new(TMActions.View, TMResources.Jobs, "View Jobs", IsBasic: true),
        new(TMActions.Create, TMResources.Jobs, "Create Jobs"),
        new(TMActions.Update, TMResources.Jobs, "Update Jobs"),
        new(TMActions.Delete, TMResources.Jobs, "Delete Jobs"),

        new(TMActions.View, TMResources.JobApplications, "View Job Applications", IsBasic: true),
        new(TMActions.Create, TMResources.JobApplications, "Create Job Applications"),
        new(TMActions.Update, TMResources.JobApplications, "Update Job Applications"),
        new(TMActions.Delete, TMResources.JobApplications, "Delete Job Applications"),

        new(TMActions.View, TMResources.Users, "View Users"),
        new(TMActions.Create, TMResources.Users, "Create Users"),
        new(TMActions.Update, TMResources.Users, "Update Users"),
        new(TMActions.Delete, TMResources.Users, "Delete Users"),

        new(TMActions.View, TMResources.Roles, "View Roles"),
        new(TMActions.Create, TMResources.Roles, "Create Roles"),
        new(TMActions.Update, TMResources.Roles, "Update Roles"),
        new(TMActions.Delete, TMResources.Roles, "Delete Roles"),

    };

    public static IReadOnlyList<FshPermission> All { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions);
    public static IReadOnlyList<FshPermission> Root { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Admin { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Basic { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => p.IsBasic).ToArray());

    public static Dictionary<string, List<FshPermission>> RolePermissions { get; } = new()
    {
        [TMRoles.Admin] = AllPermissions.Where(p => !p.IsRoot).ToList(),
        [TMRoles.Basic] = AllPermissions.Where(p => p.IsBasic).ToList(),
        [TMRoles.HR] = AllPermissions.Where(p => p.Resource == TMResources.Jobs || p.Resource == TMResources.JobApplications).ToList(),
        [TMRoles.Candidate] = AllPermissions.Where(p =>
            (p.Resource == TMResources.Jobs && p.Action == TMActions.View) ||
            (p.Resource == TMResources.JobApplications && (p.Action == TMActions.View || p.Action == TMActions.Create))
     ).ToList(),
        [TMRoles.Interviewer] = AllPermissions.Where(p =>
            p.Resource == TMResources.JobApplications && (p.Action == TMActions.View || p.Action == TMActions.Update)
     ).ToList()
    }; 
    
    public static IReadOnlyList<FshPermission> GetPermissionsForRole(string roleName)
    {
        if (RolePermissions.TryGetValue(roleName, out var permissions))
        {
            return new ReadOnlyCollection<FshPermission>(permissions);
        }
        return new ReadOnlyCollection<FshPermission>(new List<FshPermission>());
    }
}

    public record FshPermission(string Action, string Resource, string Description, bool IsBasic = false, bool IsRoot = false)
    {
        public string Name => NameFor(Action, Resource);
        public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
    }