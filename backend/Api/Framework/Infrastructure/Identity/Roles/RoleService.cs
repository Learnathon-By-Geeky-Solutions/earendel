using TalentMesh.Framework.Core.Exceptions;
using TalentMesh.Framework.Core.Identity.Roles;
using TalentMesh.Framework.Core.Identity.Roles.Features.CreateOrUpdateRole;
using TalentMesh.Framework.Core.Identity.Roles.Features.UpdatePermissions;
using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Infrastructure.Identity.Persistence;
using TalentMesh.Framework.Infrastructure.Identity.RoleClaims;
using TalentMesh.Shared.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TalentMesh.Framework.Infrastructure.Identity.Roles;

namespace TalentMesh.Framework.Infrastructure.Identity.Roles;

public class RoleService(
    RoleManager<TMRole> roleManager,
    IdentityDbContext context,
    ICurrentUser currentUser) : IRoleService
{
    private readonly RoleManager<TMRole> _roleManager = roleManager;
    private readonly IdentityDbContext _context = context;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<IEnumerable<RoleDto>> GetRolesAsync()
    {
        return await Task.Run(() => _roleManager.Roles
            .Select(role => new RoleDto { Id = role.Id, Name = role.Name!, Description = role.Description })
            .ToList());
    }

    public async Task<RoleDto?> GetRoleAsync(string id)
    {
        TMRole? role = await _roleManager.FindByIdAsync(id);
        _ = role ?? throw new NotFoundException("role not found");

        return new RoleDto { Id = role.Id, Name = role.Name!, Description = role.Description };
    }

    public async Task<RoleDto> CreateOrUpdateRoleAsync(CreateOrUpdateRoleCommand command)
    {
        TMRole? role = await _roleManager.FindByIdAsync(command.Id);

        if (role != null)
        {
            role.Name = command.Name;
            role.Description = command.Description;
            await _roleManager.UpdateAsync(role);
        }
        else
        {
            role = new TMRole(command.Name, command.Description);
            await _roleManager.CreateAsync(role);
        }

        return new RoleDto { Id = role.Id, Name = role.Name!, Description = role.Description };
    }

    public async Task DeleteRoleAsync(string id)
    {
        TMRole? role = await _roleManager.FindByIdAsync(id);
        _ = role ?? throw new NotFoundException("role not found");

        await _roleManager.DeleteAsync(role);
    }

    public async Task<RoleDto> GetWithPermissionsAsync(string id, CancellationToken cancellationToken)
    {
        var role = await GetRoleAsync(id);
        _ = role ?? throw new NotFoundException("role not found");

        role.Permissions = await _context.RoleClaims
            .Where(c => c.RoleId == id && c.ClaimType == TMClaims.Permission)
            .Select(c => c.ClaimValue!)
            .ToListAsync(cancellationToken);

        return role;
    }

    public async Task<string> UpdatePermissionsAsync(UpdatePermissionsCommand request)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId);
        _ = role ?? throw new NotFoundException("role not found");

        if (role.Name == TMRoles.Admin)
        {
            throw new TalentMeshException("operation not permitted");
        }

        // Multi-tenant check removed in single-tenant scenario

        var currentClaims = await _roleManager.GetClaimsAsync(role);

        // Remove permissions that are no longer selected
        foreach (var claim in currentClaims.Where(c => !request.Permissions.Exists(p => p == c.Value)))
        {
            var result = await _roleManager.RemoveClaimAsync(role, claim);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description).ToList();
                throw new TalentMeshException("operation failed", errors);
            }
        }

        // Add new permissions that weren't previously selected
        foreach (string permission in request.Permissions.Where(c => !currentClaims.Any(p => p.Value == c)))
        {
            if (!string.IsNullOrEmpty(permission))
            {
                _context.RoleClaims.Add(new TMRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = TMClaims.Permission,
                    ClaimValue = permission,
                    CreatedBy = _currentUser.GetUserId().ToString()
                });
                await _context.SaveChangesAsync();
            }
        }

        return "permissions updated";
    }
}
