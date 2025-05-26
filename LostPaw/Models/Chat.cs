using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostPaw.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string User1Id { get; set; }
        public string User2Id { get; set; }

        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }

        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}
