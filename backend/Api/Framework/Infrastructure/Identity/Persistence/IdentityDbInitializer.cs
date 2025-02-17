using TalentMesh.Framework.Core.Origin;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Infrastructure.Identity.RoleClaims;
using TalentMesh.Framework.Infrastructure.Identity.Roles;
using TalentMesh.Framework.Infrastructure.Identity.Users;
using TalentMesh.Shared.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TalentMesh.Framework.Infrastructure.Identity.Persistence;

using IdentityConstants = TalentMesh.Shared.Authorization.IdentityConstants;

namespace TalentMesh.Framework.Infrastructure.Identity.Persistence;

internal sealed class IdentityDbInitializer(
    ILogger<IdentityDbInitializer> logger,
    IdentityDbContext context,
    RoleManager<TMRole> roleManager,
    UserManager<TalentMeshUser> userManager,
    TimeProvider timeProvider,
    IOptions<OriginOptions> originSettings) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("Applied database migrations for identity module");
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await SeedRolesAsync();
        await SeedAdminUserAsync();
    }

    private async Task SeedRolesAsync()
    {
        foreach (string roleName in TMRoles.DefaultRoles)
        {
            // Check if the role already exists
            TMRole role = await roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                // Create a new role with a simple description (without tenant info)
                role = new TMRole(roleName, $"{roleName} Role");
                await roleManager.CreateAsync(role);
            }

            // Assign permissions based on role type
            if (roleName == TMRoles.Basic)
            {
                await AssignPermissionsToRoleAsync(context, TMPermissions.Basic, role);
            }
            else if (roleName == TMRoles.Admin)
            {
                await AssignPermissionsToRoleAsync(context, TMPermissions.Admin, role);
                // In a single-tenant scenario, you might always assign root permissions to the admin role.
                await AssignPermissionsToRoleAsync(context, TMPermissions.Root, role);
            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(IdentityDbContext dbContext, IReadOnlyList<FshPermission> permissions, TMRole role)
    {
        var currentClaims = await roleManager.GetClaimsAsync(role);
        var newClaims = permissions
            .Where(permission => !currentClaims.Any(c => c.Type == TMClaims.Permission && c.Value == permission.Name))
            .Select(permission => new TMRoleClaim
            {
                RoleId = role.Id,
                ClaimType = TMClaims.Permission,
                ClaimValue = permission.Name,
                CreatedBy = "application",
                CreatedOn = timeProvider.GetUtcNow()
            })
            .ToList();

        foreach (var claim in newClaims)
        {
            logger.LogInformation("Seeding {Role} Permission '{Permission}'.", role.Name, claim.ClaimValue);
            await dbContext.RoleClaims.AddAsync(claim);
        }

        if (newClaims.Count != 0)
        {
            await dbContext.SaveChangesAsync();
        }
    }

    private async Task SeedAdminUserAsync()
    {
        // Use a default admin email for a single-tenant scenario.
        string adminEmail = "admin@example.com";
        TalentMeshUser? adminUser = await userManager.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
        if (adminUser == null)
        {
            string adminUserName = "ADMIN";
            adminUser = new TalentMeshUser
            {
                FirstName = "Admin",
                LastName = "User",
                Email = adminEmail,
                UserName = adminUserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = adminEmail.ToUpperInvariant(),
                NormalizedUserName = adminUserName.ToUpperInvariant(),
                ImageUrl = new Uri(originSettings.Value.OriginUrl! + TenantConstants.Root.DefaultProfilePicture),
                IsActive = true
            };

            logger.LogInformation("Seeding Default Admin User.");
            var password = new PasswordHasher<TalentMeshUser>();
            adminUser.PasswordHash = password.HashPassword(adminUser, TenantConstants.DefaultPassword);
            await userManager.CreateAsync(adminUser);
        }

        if (!await userManager.IsInRoleAsync(adminUser, TMRoles.Admin))
        {
            logger.LogInformation("Assigning Admin Role to Admin User.");
            await userManager.AddToRoleAsync(adminUser, TMRoles.Admin);
        }
    }
}
