using System.Security.Claims;
using TalentMesh.Framework.Core.Identity.Users.Dtos;
using TalentMesh.Framework.Core.Identity.Users.Features.AssignUserRole;
using TalentMesh.Framework.Core.Identity.Users.Features.ChangePassword;
using TalentMesh.Framework.Core.Identity.Users.Features.ForgotPassword;
using TalentMesh.Framework.Core.Identity.Users.Features.GoogleLogin;
using TalentMesh.Framework.Core.Identity.Users.Features.RegisterUser;
using TalentMesh.Framework.Core.Identity.Users.Features.ResetPassword;
using TalentMesh.Framework.Core.Identity.Users.Features.ToggleUserStatus;
using TalentMesh.Framework.Core.Identity.Users.Features.UpdateUser;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Users.Abstractions;
public interface IUserService
{
    Task<bool> ExistsWithNameAsync(string name);
    Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null);
    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null);
    Task<List<UserDetail>> GetListAsync(CancellationToken cancellationToken);
    Task<int> GetCountAsync(CancellationToken cancellationToken);
    Task<UserDetail> GetAsync(string userId, CancellationToken cancellationToken);
    Task<bool> GetInterviewerDetailAsync(string userId, CancellationToken cancellationToken);
    Task<bool> GetHrsAsync(string? search, string? sortBy, string? sortDirection, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<UserDetail>> GetAdminsAsync(CancellationToken cancellationToken);
    Task<bool> GetInterviewersAsync(string? search, string? sortBy, string? sortDirection, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task ToggleStatusAsync(ToggleUserStatusCommand request, CancellationToken cancellationToken);
    Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
    Task<RegisterUserResponse> RegisterAsync(RegisterUserCommand request, string origin, CancellationToken cancellationToken);
    Task<GoogleLoginUserResponse> GoogleLogin(TokenRequestCommand request, string ip, string origin, CancellationToken cancellationToken);
    Task UpdateAsync(UpdateUserCommand request, string userId);
    Task DeleteAsync(string userId);
    Task<string> ConfirmEmailAsync(string userId, string code, string tenant, CancellationToken cancellationToken);
    Task<string> ConfirmPhoneNumberAsync(string userId, string code);

    // permisions
    Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default);

    // passwords
    Task ForgotPasswordAsync(ForgotPasswordCommand request, string origin, CancellationToken cancellationToken);
    Task ResetPasswordAsync(ResetPasswordCommand request, CancellationToken cancellationToken);
    Task<List<string>?> GetPermissionsAsync(string userId, CancellationToken cancellationToken);

    Task ChangePasswordAsync(ChangePasswordCommand request, string userId);
    Task<string> AssignRolesAsync(string userId, AssignUserRoleCommand request, CancellationToken cancellationToken);
    Task<List<UserRoleDetail>> GetUserRolesAsync(string userId, CancellationToken cancellationToken);
}
