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

        
        [AllowAnonymous]
        public async Task<IActionResult> Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = await _userManager.Users
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                Fullname = user.FullName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicUrl = user.ProfilePicUrl,
                UserPosts = user.Posts.Select(p => new DisplayPostListViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Type = p.Type,
                    ImageUrl = p.ImageUrl,
                    DateLostFound = p.DateLostFound,
                    Username = user.UserName
                }).ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EditProfileViewModel
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Fullname = user.FullName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicUrl = user.ProfilePicUrl,
                IsCurrentUser = true 
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel model, IFormFile ProfilePicture)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                var fileExtension = Path.GetExtension(ProfilePicture.FileName).ToLower();

                // Έλεγχος αν το αρχείο είναι εικόνα
                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".gif")
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profile_pics");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfilePicture.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Αντιγραφή του αρχείου στον φάκελο
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfilePicture.CopyToAsync(fileStream);
                    }

                    user.ProfilePicUrl = "/images/profile_pics/" + uniqueFileName; 
                }
                else
                {
                    ModelState.AddModelError("ProfilePicture", "Please upload a valid image file (jpg, jpeg, png, gif).");
                }
            }
            user.FullName = model.Fullname;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Profile", new { username = user.UserName });
            }

            return View(model);
        }
    }
}
