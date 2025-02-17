namespace TalentMesh.Framework.Core.Mail;
public interface IMailService
{
    Task SendAsync(MailRequest request, CancellationToken ct);
}
