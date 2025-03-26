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
        public async Task<string> CreateZoomMeetingAsync(string accessToken, DateTime startTime)
        {
            var requestUrl = "https://api.zoom.us/v2/users/me/meetings";

            var requestBody = new
            {
                agenda = "Zoom Meeting for YT Demo",
                default_password = false,
                duration = 60,
                password = "12345",
                start_time = startTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"), // Convert to UTC format
                timezone = "Asia/Dhaka",
                topic = "Zoom Meeting for YT Demo",
                type = 2,
                settings = new
                {
                    allow_multiple_devices = true,
                    alternative_hosts_email_notification = true,
                    breakout_room = new
                    {
                        enable = true,
                        rooms = new[]
                        {
                        new
                        {
                            name = "room1",
                            participants = new[]
                            {
                                "mdnafiulhasanhamim126@gmail.com"
                            }
                        }
                    }
                    },
                    calendar_type = 1,
                    contact_email = "mdnafiulhasanhamim12345@gmail.com",
                    contact_name = "Ajay Sharma",
                    email_notification = true,
                    encryption_type = "enhanced_encryption",
                    focus_mode = true,
                    host_video = true,
                    join_before_host = false,
                    meeting_authentication = true,
                    meeting_invitees = new[]
                    {
                    new { email = "u1904126@student.cuet.ac.bd" },
                    new { email = "mdnafiulhasanhamim12345@gmail.com" }
                },
                    authentication_exception = new[]
                    {
                    new { email = "u1904126@student.cuet.ac.bd", name = "User 1" },
                    new { email = "mdnafiulhasanhamim12345@gmail.com", name = "User 2" }
                },
                    mute_upon_entry = true,
                    participant_video = true,
                    private_meeting = true,
                    waiting_room = false,
                    watermark = false,
                    continuous_meeting_chat = new { enable = true }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            // Set the Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            _logger.LogInformation("Sending request to Zoom API: {Url}", requestUrl);

            var response = await _httpClient.PostAsync(requestUrl, jsonContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("Zoom API Response: {Response}", responseContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to create Zoom meeting. Status: {StatusCode}, Response: {Response}", response.StatusCode, responseContent);
                throw new Exception($"Zoom API error: {response.StatusCode}");
            }

            // Parse the JSON response to extract the meeting ID
            using var doc = JsonDocument.Parse(responseContent);
            if (doc.RootElement.TryGetProperty("id", out var meetingIdElement))
            {
                return meetingIdElement.GetRawText(); // Return meeting ID as a string
            }

            _logger.LogError("Zoom API response did not contain 'id'");
            throw new Exception("Zoom API response did not contain 'id'");
        }

    }
}
