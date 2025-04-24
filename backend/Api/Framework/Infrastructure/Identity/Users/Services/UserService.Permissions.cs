using System.Security.Claims;  
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Framework.Core.Exceptions;
using TalentMesh.Shared.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Services;

internal sealed partial class UserService
{
    public async Task<List<string>?> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
    {
        var permissions = await cache.GetOrSetAsync(
            GetPermissionCacheKey(userId),
            async () =>
            {
                var user = await userManager.FindByIdAsync(userId);

                _ = user ?? throw new UnauthorizedException();

                var userRoles = await userManager.GetRolesAsync(user);
                var permissions = new List<string>();
                foreach (var role in await roleManager.Roles
                    .Where(r => userRoles.Contains(r.Name!))
                    .ToListAsync(cancellationToken))
                {
                    permissions.AddRange(await db.RoleClaims
                        .Where(rc => rc.RoleId == role.Id && rc.ClaimType == TMClaims.Permission)
                        .Select(rc => rc.ClaimValue!)
                        .ToListAsync(cancellationToken));
                }
                return permissions.Distinct().ToList();
            },
            cancellationToken: cancellationToken);

        return permissions;
    }

    public async Task SeedRolePermissionsAsync(CancellationToken cancellationToken = default)
    {
        foreach (var role in TMRoles.DefaultRoles)
        {
            var roleEntity = await roleManager.FindByNameAsync(role);
            if (roleEntity == null) continue;

            // Get existing claims for this role
            var existingClaims = await roleManager.GetClaimsAsync(roleEntity);

            // Get permissions for this role from our TMPermissions class
            var rolePermissions = TMPermissions.GetPermissionsForRole(role);

            // Remove claims that are no longer valid
            foreach (var claim in existingClaims.Where(c => c.Type == TMClaims.Permission))
            {
                if (!rolePermissions.Any(p => p.Name == claim.Value))
                {
                    await roleManager.RemoveClaimAsync(roleEntity, claim);
                }
            }

            var permissionNames = rolePermissions.Select(p => p.Name).ToList();

            foreach (var permissionName in permissionNames)
            {
                if (!existingClaims.Any(c => c.Type == TMClaims.Permission && c.Value == permissionName))
                {
                    await roleManager.AddClaimAsync(roleEntity, new Claim(TMClaims.Permission, permissionName));
                }
            }

        }
    }

    public static string GetPermissionCacheKey(string userId)
    {
        return $"perm:{userId}";
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default)
    {
        var permissions = await GetPermissionsAsync(userId, cancellationToken);

        return permissions?.Contains(permission) ?? false;
    }

    public Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken)
    {
        return cache.RemoveAsync(GetPermissionCacheKey(userId), cancellationToken);
    }
}
