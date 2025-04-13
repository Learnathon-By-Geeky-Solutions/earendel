using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalentMesh.Framework.Core.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Diagnostics.CodeAnalysis;
namespace TalentMesh.Framework.Infrastructure.Mail;

[ExcludeFromCodeCoverage]
public class SmtpMailService : IMailService
{
    private readonly MailOptions _emailConfig;
    public SmtpMailService(IOptions<MailOptions> emailConfig) => _emailConfig = emailConfig.Value;
    public async Task SendEmail(MailRequest message)
    {
        var emailMessage = CreateEmailMessage(message);
        await SendAsync(emailMessage);
    }

    private async Task SendAsync(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            // await client.ConnectAsync(_emailConfig.Host, _emailConfig.Port, true);
            await client.ConnectAsync(_emailConfig.Host, _emailConfig.Port, SecureSocketOptions.SslOnConnect);

            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

            await client.SendAsync(mailMessage);
        }
        catch
        {
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }

    private MimeMessage CreateEmailMessage(MailRequest message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailConfig.DisplayName, _emailConfig.From));

        // Convert strings to MailboxAddress here (at send-time)
        emailMessage.To.AddRange(message.To.Select(x => new MailboxAddress("", x)));

        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

        return emailMessage;
    }

    private void Send(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            client.Connect(_emailConfig.Host, _emailConfig.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

            client.Send(mailMessage);
        }
        catch
        {
            //log an error message or throw an exception or both.
            throw;
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }
    }
}