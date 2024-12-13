using LostPaw.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace LostPaw.ViewModels
{
    public class UpdatePostViewModel
    {
        public int Id { get; set; }
        [Required]
        public PostType Type { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }
        [Required]
        [MaxLength(500)]
        public string? Description { get; set; }
        public string? ChipNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateLostFound { get; set; }

        public IFormFile? ImageFile { get; set; } = null;

        [Required]
        public Address? Address { get; set; }
    }
}
