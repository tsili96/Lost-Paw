using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostPaw.Models
{
    public class PetPost
    {
        public int Id { get; set; }
        [Required]
        public PostType Type { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public string ImageUrl { get; set; } //add video too
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string ChipNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateLostFound { get; set; }

    }
    public enum PostType
    {
        Lost,
        Found,
        Adoption
    }
}
