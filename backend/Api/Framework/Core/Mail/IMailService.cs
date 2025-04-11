using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Mail;
public interface IMailService
{
    // Task SendAsync(MailRequest request, CancellationToken ct);

    Task SendEmail(MailRequest message);

}
