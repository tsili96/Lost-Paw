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
        public bool IsCurrentUser { get; set; }
    }
}
