using Microsoft.AspNetCore.Http;
namespace LostPaw.ViewModels
{
    public class EditProfileViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePicUrl { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string AboutMe { get; set; }
        public bool ShowPhoneNumber { get; set; }
        public bool ShowFullName { get; set; }
        public bool IsCurrentUser { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
