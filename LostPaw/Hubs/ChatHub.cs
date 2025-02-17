using LostPaw.Models;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Plugins;

namespace LostPaw.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }
        public async Task SendMessage(string chatId,string sender, string message)
        {
            await Clients.Group(chatId).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
