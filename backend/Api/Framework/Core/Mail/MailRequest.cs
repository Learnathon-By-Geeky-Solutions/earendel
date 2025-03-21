using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Mail
{
    [ExcludeFromCodeCoverage]
    public class MailRequest
    {
        // Required properties set via the constructor.
        public Collection<string> To { get; }
        public string Subject { get; }

        // Optional properties that can be set via fluent methods or property setters.
        public string? Body { get; private set; }
        public string? From { get; private set; }
        public string? DisplayName { get; private set; }
        public string? ReplyTo { get; private set; }
        public string? ReplyToName { get; private set; }
        public Collection<string> Bcc { get; } = new Collection<string>();
        public Collection<string> Cc { get; } = new Collection<string>();
        public IDictionary<string, byte[]> AttachmentData { get; } = new Dictionary<string, byte[]>();
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        // Only required properties are required in the constructor.
        public MailRequest(Collection<string> to, string subject)
        {
            To = to;
            Subject = subject;
        }

        // Fluent methods to set optional properties.
        public MailRequest WithBody(string body)
        {
            Body = body;
            return this;
        }

        public MailRequest WithFrom(string from)
        {
            From = from;
            return this;
        }

        public MailRequest WithDisplayName(string displayName)
        {
            DisplayName = displayName;
            return this;
        }

        public MailRequest WithReplyTo(string replyTo)
        {
            ReplyTo = replyTo;
            return this;
        }

        public MailRequest WithReplyToName(string replyToName)
        {
            ReplyToName = replyToName;
            return this;
        }

        public MailRequest AddBcc(string bccAddress)
        {
            Bcc.Add(bccAddress);
            return this;
        }

        public MailRequest AddCc(string ccAddress)
        {
            Cc.Add(ccAddress);
            return this;
        }

        public MailRequest AddAttachment(string fileName, byte[] fileData)
        {
            AttachmentData[fileName] = fileData;
            return this;
        }

        public MailRequest AddHeader(string key, string value)
        {
            Headers[key] = value;
            return this;
        }
    }
}
