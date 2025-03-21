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
        public ChatHub(LostPawDbContext context)
        {
            _context = context;
        }
        public async Task JoinChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }
        public async Task SendMessage(int chatId, string senderId, string message)
        {

            var chat = await _context.Chats
                .Include(c => c.Messages)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
            {
                Console.WriteLine("❌ Chat not found!");
                return;
            }

            var sender = await _context.Users.FindAsync(senderId);
            if (sender == null)
            {
                Console.WriteLine("❌ Sender not found!");
                throw new HubException("Sender not found.");
            }

            var receiverId = chat.User1Id == senderId ? chat.User2Id : chat.User1Id;

            var chatMessage = new ChatMessage
            {
                ChatId = chatId,
                SenderId = sender.Id,
                Content = message,
                Timestamp = DateTime.UtcNow,
                IsRead = false,
                ReceiverId = receiverId
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            Console.WriteLine($"📤 Sending message from {sender.UserName} to chat {chatId}");

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", sender.UserName, message);
            await Clients.User(receiverId).SendAsync("UpdateChatList", chatId, sender.UserName, message);
        }


    }

}
