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

namespace TalentMesh.Framework.Infrastructure.Identity.Users.Services
{
    public class ExternalApiClient : IExternalApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalApiClient> _logger;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _requestUrl;
    public ExternalApiClient(HttpClient httpClient, ILogger<ExternalApiClient> logger, IConfiguration configuration)
    {
        _clientId = configuration["GithubCredentials:ClientId"];
        _clientSecret = configuration["GithubCredentials:ClientSecret"];
        _requestUrl = configuration["GithubCredentials:RequestUrl"];
        _httpClient = httpClient;
        _logger = logger;
    }

    // public async Task<string> GetAccessTokenAsync()
    // {
    //     var tokenUrl = $"https://zoom.us/oauth/token?grant_type={_grantType}&account_id={_accountId}";
    //     var request = CreateTokenRequest(tokenUrl);

    //     var response = await SendHttpRequestAsync(request).ConfigureAwait(false);
    //     string rawJson = await CleanResponseAsync(response).ConfigureAwait(false);

    //     _logger.LogDebug("Cleaned Zoom token response: {CleanJson}", rawJson);
    //     return ExtractJsonValue(rawJson, "access_token", 10);
    // }

    public async Task<string> GetAccessTokenAsync(string code)
    {
        var githubTokenUrl = $"{_requestUrl}";
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
            // Parse the response as a query string.
            var queryDict = QueryHelpers.ParseQuery(responseContent);

            if (queryDict.TryGetValue("access_token", out var token))
            {
                return token.First();
            }

            _logger.LogError("Github API response did not contain 'access_token'");
            throw new InvalidOperationException("Github API response did not contain 'access_token'");
        }
    }

}

