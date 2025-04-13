using System.Collections.ObjectModel;
using MimeKit;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Mail;

[ExcludeFromCodeCoverage]
public class MailRequest
{
    public List<string> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public MailRequest(IEnumerable<string> to, string subject, string content)
    {
        To = to.ToList();
        Subject = subject;
        Content = content;

    }
}