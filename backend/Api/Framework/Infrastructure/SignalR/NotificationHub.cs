using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using TalentMesh.Framework.Infrastructure.Identity.Users;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.SignalR
{
    [ExcludeFromCodeCoverage]

    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;
        private readonly UserManager<TMUser> _userManager; // 👈 Add UserManager


        public NotificationHub(ILogger<NotificationHub> logger, UserManager<TMUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            var userId = Context.UserIdentifier;
            var appUser = await _userManager.FindByIdAsync(userId!);

            var roles = await _userManager.GetRolesAsync(appUser!); 

            if (roles.Contains("Admin"))
            {
                
                _logger.LogInformation("Admin is connected");
                await Groups.AddToGroupAsync(Context.ConnectionId, "admin");
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
            }
            else if (userId != null)
            {
                _logger.LogInformation("User is connected");
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;

            if (userId != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{userId}");
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admin");

            await base.OnDisconnectedAsync(exception);
        }

    }

}
