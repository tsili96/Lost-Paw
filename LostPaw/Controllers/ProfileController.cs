using LostPaw.Models;
using LostPaw.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace LostPaw.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        //method to get the current user's Id
        private string GetCurrentUserId()
        {
            return _userManager.GetUserId(User);
        }


        [AllowAnonymous]
        public async Task<IActionResult> ViewProfile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // load the user along with their posts from the database
            var user = await _userManager.Users
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Id == id);

            // return 404 if user is not found
            if (user == null)
            {
                return NotFound();
            }

            var currentUserId = GetCurrentUserId();

            // map user data to ProfileViewModel
            var viewModel = new ProfileViewModel
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Fullname = user.ShowFullName ? user.FullName : null,
                PhoneNumber = user.ShowPhoneNumber ? user.PhoneNumber : null,
                ProfilePicUrl = user.ProfilePicUrl,
                AboutMe = user.AboutMe,
                ShowPhoneNumber = user.ShowPhoneNumber,
                ShowFullName = user.ShowFullName,
                UserPosts = user.Posts.Select(p => new DisplayPostListViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Type = p.Type,
                    ImageUrl = p.ImageUrl,
                    DateLostFound = p.DateLostFound,
                    Username = user.UserName
                }).ToList(),

                // check if current user views their own profile
                IsCurrentUser = currentUserId == user.Id
            };

            return View(viewModel);
        }

        //accessible only by profile owner
        public async Task<IActionResult> EditProfile()
        {
            //current user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var currentUserId = GetCurrentUserId();

            //prevention from editing other users' profiles
            if (currentUserId != user.Id)
            {
                return Forbid();
            }

            //map user data to EditProfileViewModel
            var viewModel = new EditProfileViewModel
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Fullname = user.FullName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicUrl = user.ProfilePicUrl,
                AboutMe = user.AboutMe,
                ShowPhoneNumber = user.ShowPhoneNumber,
                ShowFullName = user.ShowFullName,
                IsCurrentUser = true 
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model, IFormFile ProfilePicture)
        {
            //current user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var currentUserId = GetCurrentUserId();

            //prevention from editing other users' profiles
            if (currentUserId != user.Id)
            {
                return Forbid();
            }

            //check if the new username is already taken
            var existingUser = await _userManager.FindByNameAsync(model.Username);

            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError("Username", "This username is already taken.");
                return View(model);
            }

            //update username
            user.UserName = model.Username;

            //upload new profile picture
            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                var fileExtension = Path.GetExtension(ProfilePicture.FileName).ToLower();

                //accepted image file types
                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".gif")
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profile_pics");

                    //folder creation if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    //generate a unique file name and save the image
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfilePicture.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfilePicture.CopyToAsync(fileStream);
                    }

                    //update user's profile picture URL
                    user.ProfilePicUrl = "/images/profile_pics/" + uniqueFileName; 
                }
                else
                {
                    ModelState.AddModelError("ProfilePicture", "Please upload a valid image file (jpg, jpeg, png, gif).");
                }
            }

            //update other user data 
            user.FullName = model.Fullname;
            user.PhoneNumber = model.PhoneNumber;
            user.AboutMe = model.AboutMe;
            user.ShowPhoneNumber = model.ShowPhoneNumber;
            user.ShowFullName = model.ShowFullName;

            //save changes to db
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                //refresh sign-in to update authentication cookie with new info
                await _signInManager.RefreshSignInAsync(user);

                //redirect to the updated profile view
                return RedirectToAction("ViewProfile", new { id = user.Id });
            }

            return View(model);
        }
    }
}
