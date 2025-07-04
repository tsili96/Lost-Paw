using LostPaw.Models;
using System.ComponentModel.DataAnnotations;

namespace LostPaw.ViewModels
{
    public class DisplayPostViewModel 
    {
        public int Id { get; set; }
        public PostType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChipNumber { get; set; }
        public DateTime? DateLostFound { get; set; }
        public string ImageUrl { get; set; }
        public Address Address { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
    }
}
