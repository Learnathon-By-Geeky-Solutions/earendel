using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TalentMesh.Framework.Core.Identity.Users.Abstractions;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics.CodeAnalysis;
using TalentMesh.Framework.Infrastructure.Messaging;

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Services
{
    [ExcludeFromCodeCoverage]
    public class ExternalApiClient : IExternalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalApiClient> _logger;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _requestAccessTokenUrl;
        private readonly string _requestUserInfoUrl;

        // SSLCommerz configuration
        private readonly string _sslCommerzStoreId;
        private readonly string _sslCommerzStorePass;
        private readonly string _sslCommerzInitiateUrl;
        private readonly string _sslCommerzValidationUrl;
        private readonly string _sslCommerzSuccessUrl;
        private readonly string _sslCommerzFailUrl;
        private readonly string _sslCommerzCancelUrl;


        public ExternalApiClient(HttpClient httpClient, ILogger<ExternalApiClient> logger, IMessageBus messageBus, IConfiguration configuration)
        {
            _clientId = configuration["GithubCredentials:ClientId"]
                ?? throw new ArgumentNullException(nameof(configuration), "GithubCredentials:ClientId is missing in configuration");

            _clientSecret = configuration["GithubCredentials:ClientSecret"]
                    ?? throw new ArgumentNullException(nameof(configuration), "GithubCredentials:ClientSecret is missing in configuration");

            _requestAccessTokenUrl = configuration["GithubCredentials:RequestAccessTokenUrl"]
                    ?? throw new ArgumentNullException(nameof(configuration), "GithubCredentials:RequestAccessTokenUrl is missing in configuration");

            _requestUserInfoUrl = configuration["GithubCredentials:RequestUserInfoUrl"]
                    ?? throw new ArgumentNullException(nameof(configuration), "GithubCredentials:RequestUserInfoUrl is missing in configuration");


            _sslCommerzStoreId = configuration["SslCommerz:StoreId"]
                    ?? throw new ArgumentNullException(nameof(configuration), "SslCommerz:StoreId is missing in configuration");

            _sslCommerzStorePass = configuration["SslCommerz:StorePass"]
                    ?? throw new ArgumentNullException(nameof(configuration), "SslCommerz:StorePass is missing in configuration");


            _sslCommerzInitiateUrl = configuration["SslCommerz:InitiateUrl"]
                    ?? throw new ArgumentNullException(nameof(configuration), "SslCommerz:InitiateUrl is missing in configuration");

            _sslCommerzValidationUrl = configuration["SslCommerz:ValidationUrl"]
                    ?? throw new ArgumentNullException(nameof(configuration), "SslCommerz:ValidationUrl is missing in configuration");

            _sslCommerzSuccessUrl = configuration["SslCommerz:SuccessUrl"]
                    ?? throw new ArgumentNullException(nameof(configuration), "SslCommerz:SuccessUrl is missing in configuration");

            _sslCommerzFailUrl = configuration["SslCommerz:FailUrl"]
                    ?? throw new ArgumentNullException(nameof(configuration), "SslCommerz:FailUrl is missing in configuration");

            _sslCommerzCancelUrl = configuration["SslCommerz:CancelUrl"]
                    ?? throw new ArgumentNullException(nameof(configuration), "SslCommerz:CancelUrl is missing in configuration");


            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetAccessTokenAsync(string code)
        {
            var githubTokenUrl = $"{_requestAccessTokenUrl}";
            var requestBody = CreateAccessTokenRequestBody(code);
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _logger.LogInformation("Sending request to Github API: {Url}", githubTokenUrl);
            var response = await _httpClient.PostAsync(githubTokenUrl, jsonContent).ConfigureAwait(false);
            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            _logger.LogInformation("Github API Response: {Response}", responseContent);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get access token. Status: {StatusCode}, Response: {Response}", response.StatusCode, responseContent);
                throw new HttpRequestException($"Github API error: {response.StatusCode}");
            }
            return ExtractAccessToken(responseContent);
        }

        public async Task<(string Login, string Email, string Avatar, string ProviderKey)> GetUserInfoAsync(string accessToken)
        {
            var requestUrl = $"{_requestUserInfoUrl}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue("talentmesh-github", "1.0"));
            _logger.LogInformation("Sending GET request to GitHub user API: {Url}", requestUrl);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user info. Status: {StatusCode}, Response: {Response}", response.StatusCode, responseContent);
                throw new HttpRequestException($"GitHub API error: {response.StatusCode}");
            }
            using var jsonDoc = JsonDocument.Parse(responseContent);
            var root = jsonDoc.RootElement;
            // Handle required fields with null checks
            var login = root.GetProperty("login").GetString()
                ?? throw new InvalidOperationException("GitHub response missing login field");
            var avatar = root.GetProperty("avatar_url").GetString()
                ?? throw new InvalidOperationException("GitHub response missing avatar_url field");
            var providerKey = root.GetProperty("id").GetInt64().ToString();
            // Handle optional email field with fallback
            var email = root.TryGetProperty("email", out var emailElement)
                ? emailElement.GetString() ?? string.Empty
                : string.Empty;
            return (login, email, avatar, providerKey);
        }

        private object CreateAccessTokenRequestBody(string code)
        {
            return new
            {
                client_id = _clientId,
                client_secret = _clientSecret,
                code = code
            };
        }

        private string ExtractAccessToken(string responseContent)
        {
            var queryDict = QueryHelpers.ParseQuery(responseContent);
            if (queryDict.TryGetValue("access_token", out var token) &&
                token.Count > 0)
            {
                // Use indexer instead of First()
                var accessToken = token[0];
                if (string.IsNullOrEmpty(accessToken))
                {
                    _logger.LogError("Empty access token received");
                    throw new InvalidOperationException("Empty access token in response");
                }
                return accessToken;
            }
            _logger.LogError("Missing access_token in response: {Response}", responseContent);
            throw new InvalidOperationException("No access token found in GitHub response");
        }

        public async Task<string> InitiateSslCommerzPaymentAsync(string jobId, string amount, CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, string>
            {
                { "store_id", _sslCommerzStoreId },
                { "store_passwd", _sslCommerzStorePass },
                { "total_amount", amount },
                { "currency", "BDT" },
                { "tran_id", jobId },
                { "success_url", _sslCommerzSuccessUrl },
                { "fail_url", _sslCommerzFailUrl },
                { "cancel_url", _sslCommerzCancelUrl },
                { "cus_name", "Nafi" },
                { "cus_email", "nafi@gmail.com" },
                { "cus_phone", "01944799532" },
                { "product_name", "Watch" },
                { "product_category", "Watch" },
                { "product_profile", "Watch" },
                { "shipping_method", "No" },
                { "cus_add1", "Mirpur" },
                { "cus_city", "Dhaka" },
                { "cus_country", "BD" }
            };

            using var content = new FormUrlEncodedContent(parameters);
            var response = await _httpClient.PostAsync(_sslCommerzInitiateUrl, content, cancellationToken).ConfigureAwait(false);
            string responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("SSLCommerz Initiation Response: {Response}", responseContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to initiate SSLCommerz payment. Status: {StatusCode}", response.StatusCode);
                throw new HttpRequestException($"SSLCommerz API error: {response.StatusCode}");
            }

            // Parse the response using JsonDocument
            using var jsonDoc = JsonDocument.Parse(responseContent);
            if (jsonDoc.RootElement.TryGetProperty("GatewayPageURL", out JsonElement gatewayPageUrlElement))
            {
                string? gatewayPageUrl = gatewayPageUrlElement.GetString();

                if (string.IsNullOrEmpty(gatewayPageUrl))
                {
                    throw new InvalidOperationException("GatewayPageURL is null or empty.");
                }

                _logger.LogInformation("Successfully extracted GatewayPageURL: {GatewayPageURL}", gatewayPageUrl);

                return gatewayPageUrl;
            }
            else
            {
                _logger.LogError("Failed to extract GatewayPageURL from the response.");
                throw new InvalidOperationException("Invalid SSLCommerz initiation response: Missing GatewayPageURL.");
            }
        }

        public async Task<string> ValidateSslCommerzPaymentAsync(string valId)
        {
            // Build URL with required parameters
            var queryParams = new Dictionary<string, string?>
            {
                { "val_id", valId },
                { "store_id", _sslCommerzStoreId },
                { "store_passwd", _sslCommerzStorePass },
                { "v", "1" },
                { "format", "json" }
            };

            var url = QueryHelpers.AddQueryString(_sslCommerzValidationUrl, queryParams);

            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to validate SSLCommerz payment. Status: {StatusCode}", response.StatusCode);
                throw new HttpRequestException($"SSLCommerz Validation API error: {response.StatusCode}");
            }

            using var jsonDoc = JsonDocument.Parse(responseContent);

            if (jsonDoc.RootElement.TryGetProperty("status", out JsonElement gatewayPageUrlElement))
            {
                string? status = gatewayPageUrlElement.GetString();
                if (!string.IsNullOrEmpty(status))
                {
                    return status;
                }
                else
                {
                    _logger.LogWarning("Status property is null or empty.");
                    throw new InvalidOperationException("Validation succeeded but 'status' is null or empty.");
                }
            }
            else
            {
                _logger.LogError("Failed to extract status from the response.");
                throw new InvalidOperationException("Invalid SSLCommerz validation response: Missing status.");
            }
        }


    }
}
