using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Interviews.Application.Services;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace TalentMesh.Module.Interviews.Infrastructure.Services
{
    public class ZoomService : IZoomService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ZoomService> _logger;
        private readonly string _sdkKey;
        private readonly string _sdkSecret;
        // API URL with parameters included in the query string.
        private const string TokenUrl = "https://zoom.us/oauth/token?grant_type=account_credentials&account_id=OQaFCOsQQ2WlJhzzMVaTdw";

        public ZoomService(HttpClient httpClient, ILogger<ZoomService> logger, IConfiguration configuration)
        {
            _sdkKey = configuration["ZoomSettings:SDKKey"];
            _sdkSecret = configuration["ZoomSettings:SDKSecret"];
            _httpClient = httpClient;
            _logger = logger;

        }

        public async Task<string> GetAccessTokenAsync(string username, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, TokenUrl);
            var authInfo = $"{username}:{password}";
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get Zoom access token. Status Code: {StatusCode}", response.StatusCode);
                throw new Exception($"Zoom token request failed with status code {response.StatusCode}");
            }

            var rawJson = await response.Content.ReadAsStringAsync();

            // Ensure we remove trailing characters if needed
            rawJson = rawJson.Trim();
            if (!string.IsNullOrEmpty(rawJson) && rawJson[^1] == ';')
            {
                rawJson = rawJson.Substring(0, rawJson.Length - 1).Trim();
            }

            _logger.LogDebug("Cleaned Zoom token response: {CleanJson}", rawJson);

            // Extract access_token using Regex
            var match = Regex.Match(rawJson, @"""access_token"":\s*""([^""]+)""");
            if (!match.Success)
            {
                _logger.LogError("Failed to extract access_token from Zoom response.");
                throw new Exception("Invalid Zoom token response: Missing access token.");
            }

            string accessToken = match.Groups[1].Value;
            _logger.LogInformation("Successfully extracted Zoom access token. Expires in 3600 seconds.");

            return accessToken;
        }
   
    }
}
