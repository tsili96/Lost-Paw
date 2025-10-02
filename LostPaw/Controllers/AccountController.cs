using LostPaw.Models;
using LostPaw.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LostPaw.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] ViewModels.RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //check if email already exists
                var existingEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(model);
                }

                //check if username already exists
                var existingUser = await _userManager.FindByNameAsync(model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "This username is taken.");
                    return View(model);
                }

                //give user a default profile picture which can change later
                var defaultProfilePicUrl = "/images/profile_pics/defaultProfilePic.png";

                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    ProfilePicUrl = defaultProfilePicUrl
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                //after successful registration automatically sign in the user and redirect to homepage
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] ViewModels.LoginViewModel req)
        {
            if (ModelState.IsValid)
            {
                //find the user by email
                var user = await _userManager.FindByEmailAsync(req.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }

                //check if the password is correct and if the user is locked out
                var result = await _signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: false);

                //after successful login redirect user to homepage
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, req.RememberMe);
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // sign out the currently logged-in user and clear authentication cookie
            await _signInManager.SignOutAsync(); 
            
            //redirect user to homepage
            return RedirectToAction("Index", "Home");  
        }
    }
}

