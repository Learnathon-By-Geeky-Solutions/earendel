using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Mail;
public interface IMailService
{
    Task SendEmail(MailRequest message);

}
