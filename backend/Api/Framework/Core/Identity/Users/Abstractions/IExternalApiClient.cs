using System.Threading.Tasks;

namespace TalentMesh.Framework.Core.Identity.Users.Abstractions

{
    public interface IExternalApiClient
    {
        Task<string> GetAccessTokenAsync(string code);
        Task<(string Login, string Email, string Avatar, string ProviderKey)> GetUserInfoAsync(string accessToken);
        Task<string> InitiateSslCommerzPaymentAsync(string jobId, string amount, CancellationToken cancellationToken);
        Task<string> ValidateSslCommerzPaymentAsync(string valId);
    }

}
