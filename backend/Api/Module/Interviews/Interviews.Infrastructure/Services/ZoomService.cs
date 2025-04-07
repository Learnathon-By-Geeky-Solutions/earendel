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
using TalentMesh.Module.Interviews.Application.Services;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    public class ZoomService : IZoomService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ZoomService> _logger;
        private readonly string _sdkKey;
        private readonly string _sdkSecret;
        private readonly string _grantType;
        private readonly string _accountId;
        private readonly string _zoomUserName;
        private readonly string _zoomPassword;
        private readonly string _requestUrl;

        public ZoomService(HttpClient httpClient, ILogger<ZoomService> logger, IConfiguration configuration)
        {
            _sdkKey = configuration["ZoomSettings:SDKKey"];
            _sdkSecret = configuration["ZoomSettings:SDKSecret"];
            _grantType = configuration["ZoomSettings:GrantType"];
            _accountId = configuration["ZoomSettings:AccountId"];
            _zoomUserName = configuration["ZoomSettings:ZoomUserName"];
            _zoomPassword = configuration["ZoomSettings:ZoomPassword"];
            _requestUrl = configuration["ZoomSettings:RequestUrl"];
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var tokenUrl = $"https://zoom.us/oauth/token?grant_type={_grantType}&account_id={_accountId}";
            var request = CreateTokenRequest(tokenUrl);

            var response = await SendHttpRequestAsync(request).ConfigureAwait(false);
            string rawJson = await CleanResponseAsync(response).ConfigureAwait(false);

            _logger.LogDebug("Cleaned Zoom token response: {CleanJson}", rawJson);
            return ExtractJsonValue(rawJson, "access_token", 10);
        }

        public async Task<string> CreateZoomMeetingAsync(string accessToken, DateTime startTime)
        {
            var meetingUrl = $"{_requestUrl}/users/me/meetings";
            var requestBody = CreateMeetingRequestBody(startTime);
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _logger.LogInformation("Sending request to Zoom API: {Url}", meetingUrl);

            var response = await _httpClient.PostAsync(meetingUrl, jsonContent).ConfigureAwait(false);
            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            _logger.LogInformation("Zoom API Response: {Response}", responseContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to create Zoom meeting. Status: {StatusCode}, Response: {Response}", response.StatusCode, responseContent);
                throw new HttpRequestException($"Zoom API error: {response.StatusCode}");
            }

            return ExtractMeetingId(responseContent);
        }

        public async Task<string> GenerateSignatureAsync(string meetingNumber, int role)
        {
            ValidateSignatureParameters(meetingNumber, role);
            long iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long exp = iat + 7200; // Token expires in 2 hours

            var claims = GenerateJwtClaims(meetingNumber, role, iat, exp);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_sdkSecret)),
                    SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return await Task.FromResult(tokenHandler.WriteToken(token));
        }

        #region Helper Methods

        private HttpRequestMessage CreateTokenRequest(string tokenUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);
            var authInfo = $"{_zoomUserName}:{_zoomPassword}";
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
            return request;
        }

        private async Task<HttpResponseMessage> SendHttpRequestAsync(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("HTTP Request Failed: {Status}", response.StatusCode);
                throw new HttpRequestException($"Request failed: {response.StatusCode}");
            }
            return response;
        }

        private static async Task<string> CleanResponseAsync(HttpResponseMessage response)
        {
            string rawJson = (await response.Content.ReadAsStringAsync().ConfigureAwait(false)).Trim();
            if (!string.IsNullOrEmpty(rawJson) && rawJson[^1] == ';')
            {
                rawJson = rawJson.Substring(0, rawJson.Length - 1).Trim();
            }
            return rawJson;
        }

        private string ExtractJsonValue(string json, string key, double timeoutSeconds)
        {
            try
            {
                var match = Regex.Match(
                    json,
                    $@"""{key}"":\s*""([^""]+)""",
                    RegexOptions.None,
                    TimeSpan.FromSeconds(timeoutSeconds));

                if (!match.Success)
                {
                    _logger.LogError("Failed to extract {Key} from Zoom response.", key);
                    throw new InvalidOperationException($"Invalid Zoom token response: Missing {key}.");
                }

                _logger.LogInformation("Successfully extracted Zoom {Key}.", key);
                return match.Groups[1].Value;
            }
            catch (RegexMatchTimeoutException ex)
            {
                _logger.LogError(ex, "Regex matching timed out while extracting {Key}.", key);
                throw new TimeoutException($"Regex matching timed out while extracting {key}.", ex);
            }
        }

        private object CreateMeetingRequestBody(DateTime startTime)
        {
            return new
            {
                agenda = "Zoom Meeting for YT Demo",
                default_password = false,
                duration = 60,
                password = "12345",
                start_time = startTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"),
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
        }

        private string ExtractMeetingId(string jsonResponse)
        {
            using var doc = JsonDocument.Parse(jsonResponse);
            if (doc.RootElement.TryGetProperty("id", out var meetingIdElement))
            {
                return meetingIdElement.GetRawText();
            }
            _logger.LogError("Zoom API response did not contain 'id'");
            throw new InvalidOperationException("Zoom API response did not contain 'id'");
        }

        private static void ValidateSignatureParameters(string meetingNumber, int role)
        {
            if (string.IsNullOrEmpty(meetingNumber) || (role != 0 && role != 1))
            {
                throw new ArgumentException("Invalid meeting number or role.");
            }
        }

        private IEnumerable<Claim> GenerateJwtClaims(string meetingNumber, int role, long iat, long exp)
        {
            return new[]
            {
                new Claim("appKey", _sdkKey),
                new Claim("sdkKey", _sdkKey),
                new Claim("mn", meetingNumber),
                new Claim("role", role.ToString()),
                new Claim("iat", iat.ToString()),
                new Claim("exp", exp.ToString()),
                new Claim("tokenExp", exp.ToString())
            };
        }

        #endregion
    }
}
