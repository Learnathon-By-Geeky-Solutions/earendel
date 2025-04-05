using System.Threading.Tasks;

namespace TalentMesh.Framework.Core.Identity.Users.Abstractions

{
    public interface IExternalApiClient
{
    Task<string> GetAccessTokenAsync(string code);
    // Task<string> CreateZoomMeetingAsync(string accessToken, DateTime startTime);
    // Task<string> GenerateSignatureAsync(string meetingNumber, int role);

}

}
