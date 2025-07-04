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
        public IActionResult Index()
        {
            return View();
        }
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
        public async Task<IActionResult> Register([FromForm] ViewModels.RegisterViewModel req)
        {
            if (ModelState.IsValid)
            {
                int counter = 1;
                string baseUsername = "lostpawuser";
                string generatedUserName = baseUsername + counter;

                while (await _userManager.FindByNameAsync(generatedUserName) != null)
                {
                    counter++;
                    generatedUserName = baseUsername + counter;
                }
                var defaultProfilePicUrl = "/images/profile_pics/defaultProfilePic.png";
                var user = new User
                {
                    UserName = generatedUserName,
                    Email = req.Email,
                    FullName = req.FullName,
                    PhoneNumber = req.PhoneNumber,
                    ProfilePicUrl = defaultProfilePicUrl
                };
                var result = await _userManager.CreateAsync(user, req.Password);
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
            return View();
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
                var user = await _userManager.FindByEmailAsync(req.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: false);
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
            await _signInManager.SignOutAsync();  
            return RedirectToAction("Index", "Home");  
        }
    }
}

