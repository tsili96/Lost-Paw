using LostPaw.Data;
using LostPaw.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace LostPaw.Hubs
{
    public class ChatHub : Hub
    {
        private readonly LostPawDbContext _context;
        public async Task JoinChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }
        public async Task SendMessage(int chatId, string senderId, string message)
        {
            var sender = await _context.Users.FindAsync(senderId);
            if (sender == null)
            {
                throw new HubException("Sender not found.");
            }

            var chat = await _context.Chats.FindAsync(chatId);
            if (chat == null) return;

            var chatMessage = new ChatMessage
            {
                ChatId = chatId,
                SenderId = sender.Id,
                Content = message,
                Timestamp = DateTime.UtcNow,
                IsRead = false,
                ReceiverId = (chat.User1Id == sender.Id) ? chat.User2Id : chat.User1Id
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", sender.UserName, message);
        }

    }

}
