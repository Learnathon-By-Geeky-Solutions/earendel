using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using TalentMesh.Framework.Infrastructure.Identity.Users;

namespace TalentMesh.Framework.Infrastructure.SignalR
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            var userId = Context.UserIdentifier;

            if (user != null && user!.Claims.Any(c => c.Value == "Admin"))
            {
                _logger.LogInformation("Admin is connected");
                await Groups.AddToGroupAsync(Context.ConnectionId, "admin");
            }
            else if (userId != null)
            {
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
