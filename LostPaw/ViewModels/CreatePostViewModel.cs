using LostPaw.Models;
using System.ComponentModel.DataAnnotations;

namespace LostPaw.ViewModels
{
    public class CreatePostViewModel
    {
        [Required]
        public PostType Type { get; set; }
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        public string? ChipNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateLostFound { get; set; }

        public IFormFile ImageFile { get; set; }

        [Required]
        public Address Address { get; set; }
    }
}
