using Microsoft.AspNetCore.Mvc;
using LostPaw.Models;
using LostPaw.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LostPaw.Controllers
{
    public class ChatController : Controller
    {
        private readonly LostPawDbContext _context;

        public ChatController(LostPawDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.Name;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch the list of active chats for the user
            var chats = await _context.Chats
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .OrderByDescending(c => c.Id)  // Sort chats by most recent
                .ToListAsync();

            
            var chatViewModel = chats.Select(chat => new ChatViewModel
            {
                ChatId = chat.Id,
                User1 = chat.User1,
                User2 = chat.User2,
                LastMessage = chat.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault(),
                UnreadMessagesCount = chat.Messages.Count(m => !m.IsRead && m.ReceiverId == userId)
            }).ToList();

            return View(chatViewModel);
        }

        // open a specific chat
        public async Task<IActionResult> OpenChat(int chatId)
        {
            var chat = await _context.Chats
                .Include(c => c.User1)  
                .Include(c => c.User2)
                .Include(c => c.Messages)
                    .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
            {
                return NotFound();
            }

            // Mark all messages as read
            foreach (var message in chat.Messages.Where(m => !m.IsRead))
            {
                message.IsRead = true;
            }

            await _context.SaveChangesAsync();

            return View("ChatRoom", chat);
        }
        public async Task<IActionResult> StartChat(string recipientUsername)
        {
            if (string.IsNullOrEmpty(recipientUsername))
            {
                return BadRequest("Recipient username is required.");
            }

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var recipient = await _context.Users.FirstOrDefaultAsync(u => u.UserName == recipientUsername);

            if (recipient == null)
            {
                return NotFound("Recipient not found.");
            }

            // Check if there's an existing chat between the current user and the recipient
            var existingChat = await _context.Chats
                .Include(c => c.User1)
                .Include(c => c.User2)
                .FirstOrDefaultAsync(c =>
                    (c.User1Id == currentUser.Id && c.User2Id == recipient.Id) ||
                    (c.User1Id == recipient.Id && c.User2Id == currentUser.Id));

            if (existingChat != null)
            {
                // If a chat already exists, redirect to the existing chat room
                return RedirectToAction("OpenChat", new { chatId = existingChat.Id });
            }

            // Create a new chat if none exists
            var newChat = new Chat
            {
                User1Id = currentUser.Id,
                User2Id = recipient.Id
            };

            _context.Chats.Add(newChat);
            await _context.SaveChangesAsync();

            // Redirect to the newly created chat room
            return RedirectToAction("OpenChat", new { chatId = newChat.Id });
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(int chatId, string message)
        {
            var senderId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;

            if (string.IsNullOrEmpty(message))
            {
                return BadRequest("Message cannot be empty.");
            }

            var chat = await _context.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
            {
                return NotFound("Chat not found.");
            }

            var newMessage = new ChatMessage
            {
                ChatId = chatId,
                SenderId = senderId,
                Content = message,
                Timestamp = DateTime.UtcNow,
                IsRead = false,
                ReceiverId = (chat.User1Id == senderId) ? chat.User2Id : chat.User1Id
            };

            _context.ChatMessages.Add(newMessage);
            await _context.SaveChangesAsync();

            return RedirectToAction("OpenChat", new { chatId });
        }


    }
}
