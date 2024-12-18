using LostPaw.Models;
using System.ComponentModel.DataAnnotations;

namespace LostPaw.ViewModels
{
    public class DisplayPostListViewModel
    {
        public int Id { get; set; }
        public PostType Type { get; set; }
        public string Title { get; set; }
        public DateTime? DateLostFound { get; set; }
        public string ImageUrl { get; set; }
        public string Username { get; set; }
    }
}
