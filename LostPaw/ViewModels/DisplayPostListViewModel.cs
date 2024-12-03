using LostPaw.Models;
using System.ComponentModel.DataAnnotations;

namespace LostPaw.ViewModels
{
    public class DisplayPostListViewModel
    {
        public PostType Type { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateLostFound { get; set; }

        public IFormFile ImageFile { get; set; }

    }
}
