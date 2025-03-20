using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Text;
using Finbuckle.MultiTenant.Abstractions;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Framework.Core.Exceptions;
using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using TalentMesh.Framework.Core.Identity.Users.Dtos;
using TalentMesh.Framework.Core.Identity.Users.Features.AssignUserRole;
using TalentMesh.Framework.Core.Identity.Users.Features.RegisterUser;
using TalentMesh.Framework.Core.Identity.Users.Features.ToggleUserStatus;
using TalentMesh.Framework.Core.Identity.Users.Features.UpdateUser;
using TalentMesh.Framework.Core.Jobs;
using TalentMesh.Framework.Core.Mail;
using TalentMesh.Framework.Core.Storage;
using TalentMesh.Framework.Core.Storage.File;
using TalentMesh.Framework.Infrastructure.Constants;
using TalentMesh.Framework.Infrastructure.Identity.Persistence;
using TalentMesh.Framework.Infrastructure.Identity.Roles;
using TalentMesh.Framework.Infrastructure.Tenant;
using TalentMesh.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Auth;
using TalentMesh.Framework.Core.Identity.Tokens;
using TalentMesh.Framework.Core.Identity.Tokens.Features.Generate;
using Microsoft.AspNetCore.Http;
using TalentMesh.Framework.Core.Identity.Users.Features.GoogleLogin;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Google.Apis.Auth.OAuth2.Responses;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Services;
[ExcludeFromCodeCoverage]

internal sealed partial class UserService(
    UserManager<TMUser> userManager,
    SignInManager<TMUser> signInManager,
    RoleManager<TMRole> roleManager,
    IdentityDbContext db,
    ICacheService cache,
    IJobService jobService,
    IMailService mailService,
    IMultiTenantContextAccessor<TMTenantInfo> multiTenantContextAccessor,
    IStorageService storageService,
    ITokenService tokenService
    ) : IUserService
{
    private const string UserNotFoundMessage = "user not found";

    private void EnsureValidTenant()
    {
        if (string.IsNullOrWhiteSpace(multiTenantContextAccessor?.MultiTenantContext?.TenantInfo?.Id))
        {
            throw new UnauthorizedException("invalid tenant");
        }
    }

    public Task<string> ConfirmEmailAsync(string userId, string code, string tenant, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> ConfirmPhoneNumberAsync(string userId, string code)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
    {
        EnsureValidTenant();
        return await userManager.FindByEmailAsync(email.Normalize()) is TMUser user && user.Id != exceptId;
    }

    public async Task<bool> ExistsWithNameAsync(string name)
    {
        EnsureValidTenant();
        return await userManager.FindByNameAsync(name) is not null;
    }

    public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
    {
        EnsureValidTenant();
        return await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is TMUser user && user.Id != exceptId;
    }

    public async Task<UserDetail> GetAsync(string userId, CancellationToken cancellationToken)
    {
        var userDetail = await (from user in userManager.Users
                                where user.Id == userId
                                select new UserDetail
                                {
                                    Id = Guid.Parse(user.Id),
                                    UserName = user.UserName,
                                    Email = user.Email,
                                    IsActive = user.IsActive,
                                    EmailConfirmed = user.EmailConfirmed,
                                    ImageUrl = user.ImageUrl,
                                    Roles = (from ur in db.UserRoles
                                             join r in db.Roles on ur.RoleId equals r.Id
                                             where ur.UserId == userId
                                             select r.Name).ToList()
                                })
                                .AsNoTracking()
                                .FirstOrDefaultAsync(cancellationToken);

        if (userDetail is null)
        {
            throw new NotFoundException(UserNotFoundMessage);
        }

        return userDetail;
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken) =>
        userManager.Users.AsNoTracking().CountAsync(cancellationToken);

    public async Task<List<UserDetail>> GetListAsync(CancellationToken cancellationToken)
    {
        var users = await userManager.Users.AsNoTracking().ToListAsync(cancellationToken);
        return users.Adapt<List<UserDetail>>();
    }

    public Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        throw new NotImplementedException();
    }

    public async Task<RegisterUserResponse> RegisterAsync(RegisterUserCommand request, string origin, CancellationToken cancellationToken)
    {
        // create user entity
        var user = new TMUser
        {
            Email = request.Email,
            // FirstName = request.FirstName,
            // LastName = request.LastName,
            UserName = request.UserName,
            // PhoneNumber = request.PhoneNumber,
            IsActive = true,
            EmailConfirmed = true
        };

        // register user
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new TalentMeshException("error while registering a new user", errors);
        }

        await userManager.AddToRoleAsync(user, request.Role.ToString());

        // send confirmation mail
        if (!string.IsNullOrEmpty(user.Email))
        {
            string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
            var mailRequest = new MailRequest(
                new Collection<string> { user.Email },
                "Confirm Registration",
                emailVerificationUri);
            jobService.Enqueue("email", () => mailService.SendAsync(mailRequest, CancellationToken.None));
        }

        return new RegisterUserResponse(user.Id);
    }

    public async Task<GoogleLoginUserResponse> GoogleLogin(TokenRequestCommand request, string ip, string origin, CancellationToken cancellationToken)
    {
        try
        {

            // Validate Google token
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
            var email = payload.Email;
            var providerKey = payload.Subject; // Google unique user ID
            // Check if user already exists
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                // Check if user is an external login
                var logins = await userManager.GetLoginsAsync(existingUser);
                if (logins.Any(l => l.LoginProvider == "Google"))
                {
                    Console.WriteLine("inside loginsss");
                    var tokenGenerationCommandForExistingUser = new TokenGenerationCommand(email, null); // Pass email and password

                    // Generate JWT token for existing user
                    var tokenResponseForExistingUser = await tokenService.GenerateTokenAsync(
                        tokenGenerationCommandForExistingUser,
                        ip,
                        cancellationToken
                    );

                    return new GoogleLoginUserResponse(existingUser.Id, tokenResponseForExistingUser.Token, tokenResponseForExistingUser.RefreshToken, tokenResponseForExistingUser.Roles);
                }
                else
                {
                    return new GoogleLoginUserResponse("Email is already registered with a different method.", "", "", []);
                }
            }

            // Create new user WITHOUT a password
            var newUser = new TMUser
            {
                Email = email,
                UserName = email,
                IsActive = true,
                ImageUrl = new Uri(payload.Picture),
                EmailConfirmed = true
            };

            var createUserResult = await userManager.CreateAsync(newUser);
            if (!createUserResult.Succeeded)
            {
                return new GoogleLoginUserResponse("User creation failed", "", "", []);
            }

            // Link Google account to this user
            var loginInfo = new UserLoginInfo("Google", providerKey, "Google");
            var addLoginResult = await userManager.AddLoginAsync(newUser, loginInfo);
            if (!addLoginResult.Succeeded)
            {
                return new GoogleLoginUserResponse("Failed to add external login", "", "", []);
            }

            // Assign default role
            await userManager.AddToRoleAsync(newUser, TMRoles.Candidate);

            // Generate JWT for the new user
            var tokenGenerationCommand = new TokenGenerationCommand(email, null); // Pass email and password

            // Generate JWT token for existing user
            var tokenResponse = await tokenService.GenerateTokenAsync(
             tokenGenerationCommand,
             ip,
             cancellationToken
         );

            return new GoogleLoginUserResponse(newUser.Id, tokenResponse.Token, tokenResponse.RefreshToken, tokenResponse.Roles);
        }
        catch (Exception ex)
        {
            return new GoogleLoginUserResponse($"Error: {ex.Message}", "", "", []);
        }
    }


    public async Task ToggleStatusAsync(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(UserNotFoundMessage);

        bool isAdmin = await userManager.IsInRoleAsync(user, TMRoles.Admin);
        if (isAdmin)
        {
            throw new TalentMeshException("Administrators Profile's Status cannot be toggled");
        }

        user.IsActive = request.ActivateUser;

        await userManager.UpdateAsync(user);
    }

    public async Task UpdateAsync(UpdateUserCommand request, string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(UserNotFoundMessage);

        Uri imageUri = user.ImageUrl ?? null!;
        if (request.Image != null || request.DeleteCurrentImage)
        {
            user.ImageUrl = await storageService.UploadAsync<TMUser>(request.Image, FileType.Image);
            if (request.DeleteCurrentImage && imageUri != null)
            {
                storageService.Remove(imageUri);
            }
        }

        user.PhoneNumber = request.PhoneNumber;
        string? phoneNumber = await userManager.GetPhoneNumberAsync(user);
        if (request.PhoneNumber != phoneNumber)
        {
            await userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
        }

        var result = await userManager.UpdateAsync(user);
        await signInManager.RefreshSignInAsync(user);

        if (!result.Succeeded)
        {
            throw new TalentMeshException("Update profile failed");
        }
    }

    public async Task DeleteAsync(string userId)
    {
        TMUser? user = await userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(UserNotFoundMessage);

        user.IsActive = false;
        IdentityResult? result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            List<string> errors = result.Errors.Select(error => error.Description).ToList();
            throw new TalentMeshException("Delete profile failed", errors);
        }
    }

    private async Task<string> GetEmailVerificationUriAsync(TMUser user, string origin)
    {
        EnsureValidTenant();

        string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string route = "api/users/confirm-email/";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
        verificationUri = QueryHelpers.AddQueryString(verificationUri,
            TenantConstants.Identifier,
            multiTenantContextAccessor?.MultiTenantContext?.TenantInfo?.Id!);
        return verificationUri;
    }

    public async Task<string> AssignRolesAsync(string userId, AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = await userManager.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(UserNotFoundMessage);

        // Check if disabling the admin role for an admin user
        if (await userManager.IsInRoleAsync(user, TMRoles.Admin) &&
            request.UserRoles.Any(r => !r.Enabled && r.RoleName == TMRoles.Admin))
        {
            int adminCount = (await userManager.GetUsersInRoleAsync(TMRoles.Admin)).Count;

            if (user.Email == TenantConstants.Root.EmailAddress &&
                multiTenantContextAccessor?.MultiTenantContext?.TenantInfo?.Id == TenantConstants.Root.Id)
            {
                throw new TalentMeshException("action not permitted");
            }
            else if (adminCount <= 2)
            {
                throw new TalentMeshException("tenant should have at least 2 admins.");
            }
        }

        // Process roles to add
        var rolesToAdd = request.UserRoles
       .Where(r => r.Enabled)
       .Select(r => r.RoleName!)
       .ToList();

        foreach (var roleName in rolesToAdd)
        {
            if (await roleManager.FindByNameAsync(roleName) is not null &&
                !await userManager.IsInRoleAsync(user, roleName))
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }


        // Process roles to remove
        await Task.WhenAll(
         request.UserRoles
             .Where(r => !r.Enabled)
             .Select(async r =>
                 await roleManager.FindByNameAsync(r.RoleName!) is not null
                     ? userManager.RemoveFromRoleAsync(user, r.RoleName!)
                     : Task.CompletedTask)
     );


        return "User Roles Updated Successfully.";
    }


    public async Task<List<UserRoleDetail>> GetUserRolesAsync(string userId, CancellationToken cancellationToken)
    {
        var userRoles = new List<UserRoleDetail>();

        var user = await userManager.FindByIdAsync(userId);
        if (user is null) throw new NotFoundException(UserNotFoundMessage);
        var roles = await roleManager.Roles.AsNoTracking().ToListAsync(cancellationToken);
        if (roles is null) throw new NotFoundException("roles not found");
        foreach (var role in roles)
        {
            userRoles.Add(new UserRoleDetail
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Description = role.Description,
                Enabled = await userManager.IsInRoleAsync(user, role.Name!)
            });
        }

        return userRoles;
    }
}
