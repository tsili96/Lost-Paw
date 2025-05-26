using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostPaw.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public bool IsRead { get; set; } = false;
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
