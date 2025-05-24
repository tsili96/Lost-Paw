namespace LostPaw.Models
{
    public class ChatViewModel
    {
        public int ChatId { get; set; }
        public User User1 { get; set; }
        public User User2 { get; set; }
        public ChatMessage LastMessage { get; set; }
        public int UnreadMessagesCount { get; set; }
    }
}
