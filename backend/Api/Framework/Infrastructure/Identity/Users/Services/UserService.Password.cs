﻿using System.Collections.ObjectModel;
using System.Text;
using TalentMesh.Framework.Core.Exceptions;
using TalentMesh.Framework.Core.Identity.Users.Features.ChangePassword;
using TalentMesh.Framework.Core.Identity.Users.Features.ForgotPassword;
using TalentMesh.Framework.Core.Identity.Users.Features.ResetPassword;
using TalentMesh.Framework.Core.Mail;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Services;

internal sealed partial class UserService
{
    public async Task ForgotPasswordAsync(ForgotPasswordCommand request, string origin, CancellationToken cancellationToken)
    {
        EnsureValidTenant();

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new NotFoundException("user not found");
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new InvalidOperationException("user email cannot be null or empty");
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var resetPasswordUri = $"{origin}/reset-password?token={token}&email={request.Email}";

        var mailRequest = new MailRequest(
            new Collection<string> { user.Email },
            "Reset Password",
            $"Please reset your password using the following link: {resetPasswordUri}");

        jobService.Enqueue(() => mailService.SendEmail(mailRequest));
    }

    public async Task ResetPasswordAsync(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        EnsureValidTenant();

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new NotFoundException("user not found");
        }

        request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
        var result = await userManager.ResetPasswordAsync(user, request.Token, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            throw new TalentMeshException("error resetting password", errors);
        }
    }

    public async Task ChangePasswordAsync(ChangePasswordCommand request, string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException("user not found");

        var result = await userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            throw new TalentMeshException("failed to change password", errors);
        }
    }
}
