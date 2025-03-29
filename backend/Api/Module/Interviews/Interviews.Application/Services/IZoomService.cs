using System.Threading.Tasks;

namespace TalentMesh.Module.Interviews.Application.Services
{
    public interface IZoomService
    {
        /// <summary>
        /// Gets a Zoom access token using Basic Authentication.
        /// </summary>
        /// <param name="username">Zoom API username for Basic Auth.</param>
        /// <param name="password">Zoom API password for Basic Auth.</param>
        /// <returns>The access token as a string.</returns>
        Task<string> GetAccessTokenAsync();
        Task<string> CreateZoomMeetingAsync(string accessToken, DateTime startTime);
        Task<string> GenerateSignatureAsync(string meetingNumber, int role);

    }
}
