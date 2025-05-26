using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostPaw.Models
{
    public class PetPost
    {
        public int Id { get; set; }
        public PostType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public string ImageUrl { get; set; }
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
