using Npgsql;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Text;
using System.Net;
using Finbuckle.MultiTenant;
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
using TalentMesh.Framework.Core.Identity.Users.Features.GithubLogin;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Google.Apis.Auth.OAuth2.Responses;
using System.Diagnostics.CodeAnalysis;
using TalentMesh.Framework.Infrastructure.Messaging;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Services;

[ExcludeFromCodeCoverage]
public class IdentityServices
{
    public UserManager<TMUser> UserManager { get; }
    public SignInManager<TMUser> SignInManager { get; }
    public IHttpContextAccessor HttpContextAccessor { get; }
    public RoleManager<TMRole> RoleManager { get; }
    public IdentityDbContext Db { get; }
    public IExternalApiClient ApiClient { get; }

    public IdentityServices(
        UserManager<TMUser> userManager,
        SignInManager<TMUser> signInManager,
        RoleManager<TMRole> roleManager,
        IHttpContextAccessor httpContextAccessor,
        IdentityDbContext db,
        IExternalApiClient apiClient)
    {
        UserManager = userManager;
        SignInManager = signInManager;
        RoleManager = roleManager;
        HttpContextAccessor = httpContextAccessor;
        Db = db;
        ApiClient = apiClient;
    }
}

// Grouping other infrastructure-related dependencies
[ExcludeFromCodeCoverage]
public class InfrastructureServices
{
    public ICacheService CacheService { get; }
    public IJobService JobService { get; }
    public IMailService MailService { get; }
    public IMultiTenantContextAccessor<TMTenantInfo> MultiTenantContextAccessor { get; }
    public IStorageService StorageService { get; }
    public IMessageBus MessageBus { get; }
    public ITokenService TokenService { get; }

    public InfrastructureServices(
        ICacheService cacheService,
        IJobService jobService,
        IMailService mailService,
        IMultiTenantContextAccessor<TMTenantInfo> multiTenantContextAccessor,
        IStorageService storageService,
        IMessageBus messageBus,
        ITokenService tokenService)
    {
        CacheService = cacheService;
        JobService = jobService;
        MailService = mailService;
        MultiTenantContextAccessor = multiTenantContextAccessor;
        StorageService = storageService;
        MessageBus = messageBus;
        TokenService = tokenService;
    }
}

[ExcludeFromCodeCoverage]

internal sealed partial class UserService(
      IdentityServices identityServices,
        InfrastructureServices infrastructureServices
    ) : IUserService
{
    // Expose dependencies through private fields for clarity
    private readonly UserManager<TMUser> userManager = identityServices.UserManager;
    private readonly SignInManager<TMUser> signInManager = identityServices.SignInManager;
    private readonly RoleManager<TMRole> roleManager = identityServices.RoleManager;
    private readonly IdentityDbContext db = identityServices.Db;
    private readonly IExternalApiClient apiClient = identityServices.ApiClient;

    private readonly IHttpContextAccessor httpContextAccessor = identityServices.HttpContextAccessor;


    private readonly ICacheService cache = infrastructureServices.CacheService;
    private readonly IJobService jobService = infrastructureServices.JobService;
    private readonly IMailService mailService = infrastructureServices.MailService;
    private readonly IMultiTenantContextAccessor<TMTenantInfo> multiTenantContextAccessor = infrastructureServices.MultiTenantContextAccessor;
    private readonly IStorageService storageService = infrastructureServices.StorageService;
    private readonly IMessageBus messageBus = infrastructureServices.MessageBus;
    private readonly ITokenService tokenService = infrastructureServices.TokenService;


    private const string UserNotFoundMessage = "user not found";

    private void EnsureValidTenant()
    {
        if (string.IsNullOrWhiteSpace(multiTenantContextAccessor?.MultiTenantContext?.TenantInfo?.Id))
        {
            throw new UnauthorizedException("invalid tenant");
        }
    }

    public async Task<string> ConfirmEmailAsync(
    string userId,
    string code,
    string tenacy,
    CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(userId))
            return "User ID is required.";
        if (string.IsNullOrEmpty(code))
            return "Confirmation code is required.";
        if (string.IsNullOrEmpty(tenacy))
            return "Tenant is required.";

        // Find the user by ID. (Make sure userManager is properly injected.)
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return "User not found.";
        }

        // Check tenant - if you have tenant logic, verify tenant is correct.
        if (user.TenantId != tenacy)
        {
            return "Invalid tenant.";
        }

        // Check if the email is already confirmed.
        if (user.EmailConfirmed)
        {
            return "Email already confirmed.";
        }

        // Decode the token (it was encoded with Base64UrlEncode)
        string decodedCode;
        try
        {
            var codeBytes = WebEncoders.Base64UrlDecode(code);
            decodedCode = Encoding.UTF8.GetString(codeBytes);
        }
        catch (Exception)
        {
            return "Invalid confirmation code format.";
        }


        // Confirm the email using the Identity API.
        var result = await userManager.ConfirmEmailAsync(user, decodedCode);
        if (!result.Succeeded)
        {
            return string.Join(", ", result.Errors.Select(e => e.Description));
        }

        return "Email confirmed successfully!";
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
        var userDetail = await GetUserDetailAsync(userId, cancellationToken);

        if (userDetail is null)
        {
            throw new NotFoundException(UserNotFoundMessage);
        }

        return userDetail;
    }

    public async Task<bool> GetInterviewerDetailAsync(string userId, CancellationToken cancellationToken)
    {
        var userDetail = await GetUserDetailAsync(userId, cancellationToken);

        if (userDetail is null)
        {
            throw new NotFoundException(UserNotFoundMessage);
        }

        await PublishInterviewerDetailEvent(userDetail);
        return true;
    }

    private async Task<UserDetail?> GetUserDetailAsync(string userId, CancellationToken cancellationToken)
    {
        var userDetail = await (from user in userManager.Users
                                where user.Id == userId
                                join ur in db.UserRoles on user.Id equals ur.UserId into userRoles
                                from ur in userRoles.DefaultIfEmpty()
                                join r in db.Roles on ur.RoleId equals r.Id into roles
                                from role in roles.DefaultIfEmpty()
                                group role by new
                                {
                                    user.Id,
                                    user.UserName,
                                    user.Email,
                                    user.IsActive,
                                    user.EmailConfirmed,
                                    user.ImageUrl
                                } into g
                                select new UserDetail
                                {
                                    Id = Guid.Parse(g.Key.Id),
                                    UserName = g.Key.UserName,
                                    Email = g.Key.Email,
                                    IsActive = g.Key.IsActive,
                                    EmailConfirmed = g.Key.EmailConfirmed,
                                    ImageUrl = g.Key.ImageUrl,
                                    Roles = g
                                    .Select(r => r.Name)
                                    .Where(name => name != null)
                                    .Select(name => name!)  // null-forgiving, safe after filtering
                                    .Distinct()
                                    .ToList()
                                })
                                .AsNoTracking()
                                .FirstOrDefaultAsync(cancellationToken);

        return userDetail;
    }


    public async Task<bool> GetHrsAsync(
        string? search,
        string? sortBy,
        string? sortDirection,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var baseQuery = BuildBaseQuery("HR");
        var filteredQuery = ApplySearchFilter(baseQuery, search);
        var sortedQuery = ApplySorting(filteredQuery, sortBy, sortDirection);

        var (totalRecords, hrs) = await ExecutePaginatedQuery(sortedQuery, pageNumber, pageSize, cancellationToken);

        await PublishHrFetchedEvent(totalRecords, hrs, sortBy, sortDirection);
        return true;
    }
    public async Task<List<UserDetail>> GetAdminsAsync(CancellationToken cancellationToken)
    {
        var baseQuery = BuildBaseQuery("Admin");
        return await baseQuery.ToListAsync(cancellationToken);
    }

    public async Task<bool> GetInterviewersAsync(
        string? search,
        string? sortBy,
        string? sortDirection,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var baseQuery = BuildBaseQuery("Interviewer");
        var filteredQuery = ApplySearchFilter(baseQuery, search);
        var sortedQuery = ApplySorting(filteredQuery, sortBy, sortDirection);

        var (totalRecords, interviewers) = await ExecutePaginatedQuery(sortedQuery, pageNumber, pageSize, cancellationToken);

        await PublishInterviewerFormFetchedEvent(totalRecords, interviewers, sortBy, sortDirection);
        return true;
    }

    private IQueryable<UserDetail> BuildBaseQuery(string role)
    {
        return from u in db.Users
               join ur in db.UserRoles on u.Id equals ur.UserId
               join r in db.Roles on ur.RoleId equals r.Id
               where r.Name == role
               select new UserDetail
               {
                   Id = Guid.Parse(u.Id),
                   UserName = u.UserName,
                   Email = u.Email,
                   IsActive = u.IsActive,
                   EmailConfirmed = u.EmailConfirmed,
                   ImageUrl = u.ImageUrl,
                   Roles = new List<string> { role }
               };
    }

    private static IQueryable<UserDetail> ApplySearchFilter(IQueryable<UserDetail> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search)) return query;

        return query.Where(user =>
            (!string.IsNullOrWhiteSpace(user.UserName) &&
                user.UserName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrWhiteSpace(user.Email) &&
                user.Email.Contains(search, StringComparison.OrdinalIgnoreCase))
        );

    }

    private static IQueryable<UserDetail> ApplySorting(
    IQueryable<UserDetail> query,
    string? sortBy,
    string? sortDirection)
    {
        var isAscending = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);

        return (sortBy?.ToLower()) switch
        {
            "name" => isAscending ?
                query.OrderBy(user => user.UserName) :
                query.OrderByDescending(user => user.UserName),
            "email" => isAscending ?
                query.OrderBy(user => user.Email) :
                query.OrderByDescending(user => user.Email),
            _ => query.OrderBy(user => user.UserName)
        };
    }

    private static async Task<(int TotalRecords, List<UserDetail> Users)> ExecutePaginatedQuery(
    IQueryable<UserDetail> query,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken)
    {
        var totalRecords = await query.CountAsync(cancellationToken);

        var results = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (totalRecords, results);
    }

    private async Task PublishHrFetchedEvent(
    int totalRecords,
    List<UserDetail> hrs,
    string? sortBy,
    string? sortDirection)
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userId = user?.GetUserId();

        var payload = new
        {
            EventType = "hr.list.fetched",
            Timestamp = DateTime.UtcNow,
            RequestedBy = userId,
            SortBy = sortBy,
            SortDirection = sortDirection,
            TotalRecords = totalRecords,
            Hrs = hrs.Select(hr => new
            {
                hr.Id,
                hr.UserName,
                hr.Email,
                hr.IsActive,
                hr.EmailConfirmed,
                hr.ImageUrl,
                hr.Roles
            }).ToList()
        };

        await messageBus.PublishAsync(
            payload,
            "hr.job.list.events",
            "hr.job.list.fetched",
            CancellationToken.None
        );
    }

    private async Task PublishInterviewerDetailEvent(
    UserDetail interviewer)
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userId = user?.GetUserId();

        var payload = new
        {
            EventType = "interviewer.detail.fetched",
            Timestamp = DateTime.UtcNow,
            RequestedBy = userId,
            Interviewer = new
            {
                interviewer.Id,
                interviewer.UserName,
                interviewer.Email,
                interviewer.ImageUrl,
            }
        };

        Console.WriteLine(payload);

        await messageBus.PublishAsync(
            payload,
            "interviewer.detail.events",
            "interviewer.detail.fetched",
            CancellationToken.None
        );
    }

    private async Task PublishInterviewerFormFetchedEvent(
    int totalRecords,
    List<UserDetail> interviewers,
    string? sortBy,
    string? sortDirection)
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userId = user?.GetUserId();

        var payload = new
        {
            EventType = "interviewer.list.fetched",
            Timestamp = DateTime.UtcNow,
            RequestedBy = userId,
            SortBy = sortBy,
            SortDirection = sortDirection,
            TotalRecords = totalRecords,
            Interviewers = interviewers.Select(interviewer => new
            {
                interviewer.Id,
                interviewer.UserName,
                interviewer.Email,
                interviewer.IsActive,
                interviewer.EmailConfirmed,
                interviewer.ImageUrl,
                interviewer.Roles
            }).ToList()
        };

        await messageBus.PublishAsync(
            payload,
            "interviewer.form.list.events",
            "interviewer.form.list.fetched",
            CancellationToken.None
        );
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken) =>
        userManager.Users.AsNoTracking().CountAsync(cancellationToken);

    public async Task<List<UserDetail>> GetListAsync(CancellationToken cancellationToken)
    {
        // var usersWithRoles = await userManager.Users
        //     .Include(u => u.UserRoles!)              // bring in the join table
        //         .ThenInclude(ur => ur.Role!)        // bring in each Role
        //     .AsNoTracking()
        //     .ToListAsync(cancellationToken);

        // // Now Mapster will see the UserRoles.Role.Name and fill your DTO’s Roles
        // return usersWithRoles.Adapt<List<UserDetail>>();


        var userDetail = await (from user in userManager.Users
                                join ur in db.UserRoles on user.Id equals ur.UserId into userRoles
                                from ur in userRoles.DefaultIfEmpty()
                                join r in db.Roles on ur.RoleId equals r.Id into roles
                                from role in roles.DefaultIfEmpty()
                                where role != null && role.Name == "Interviewer"
                                group role by new
                                {
                                    user.Id,
                                    user.UserName,
                                    user.Email,
                                    user.IsActive,
                                    user.EmailConfirmed,
                                    user.ImageUrl
                                } into g
                                select new UserDetail
                                {
                                    Id = Guid.Parse(g.Key.Id),
                                    UserName = g.Key.UserName,
                                    Email = g.Key.Email,
                                    IsActive = g.Key.IsActive,
                                    EmailConfirmed = g.Key.EmailConfirmed,
                                    ImageUrl = g.Key.ImageUrl,
                                    Roles = g
                                    .Select(r => r.Name)
                                    .Where(name => name != null)
                                    .Select(name => name!)  // null-forgiving, safe after filtering
                                    .Distinct()
                                    .ToList()
                                })
                                .AsNoTracking()
                                .ToListAsync(cancellationToken);

        return userDetail;
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
            EmailConfirmed = false
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
            jobService.Enqueue("email", () => mailService.SendEmail(mailRequest));
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
                if (logins.Any(l => l.LoginProvider == AuthProviders.Google))
                {
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
            var loginInfo = new UserLoginInfo(AuthProviders.Google, providerKey, AuthProviders.GitHub);
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

    public async Task<GoogleLoginUserResponse> GithubLogin(GithubRequestCommand request, string ip, string origin, CancellationToken cancellationToken)
    {
        string accessToken = await apiClient.GetAccessTokenAsync(request.Code);

        var (Login, Email, Avatar, ProviderKey) = await apiClient.GetUserInfoAsync(accessToken);

        try
        {

            // Check if user already exists
            var existingUser = await userManager.FindByEmailAsync(Email);

            if (existingUser != null)
            {
                // Check if user is an external login
                var logins = await userManager.GetLoginsAsync(existingUser);
                if (logins.Any(l => l.LoginProvider == AuthProviders.GitHub))
                {
                    var tokenGenerationCommandForExistingUser = new TokenGenerationCommand(Email, null); // Pass email and password

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
                Email = Email,
                UserName = Login,
                IsActive = true,
                ImageUrl = new Uri(Avatar),
                EmailConfirmed = true
            };

            var createUserResult = await userManager.CreateAsync(newUser);
            if (!createUserResult.Succeeded)
            {
                return new GoogleLoginUserResponse("User creation failed", "", "", []);
            }

            // Link Google account to this user
            var loginInfo = new UserLoginInfo(AuthProviders.GitHub, ProviderKey, AuthProviders.GitHub);
            var addLoginResult = await userManager.AddLoginAsync(newUser, loginInfo);
            if (!addLoginResult.Succeeded)
            {
                return new GoogleLoginUserResponse("Failed to add external login", "", "", []);
            }

            // Assign default role
            await userManager.AddToRoleAsync(newUser, TMRoles.Candidate);

            // Generate JWT for the new user
            var tokenGenerationCommand = new TokenGenerationCommand(Email, null); // Pass email and password

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
            "tenacy",
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
