using Microsoft.AspNetCore.Mvc;
using LostPaw.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using LostPaw.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using LostPaw.ViewModels;

namespace LostPaw.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly LostPawDbContext _context;
        private readonly UserManager<User> _userManager;

        public PostsController(LostPawDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        [AllowAnonymous] 
        public IActionResult Index()
        {
            var posts = _context.Posts.ToList();
            return View(posts);  
        }

       
        [HttpGet]
        public IActionResult Create()
        {
            return View("CreatePost");  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                //IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
              
                //foreach (var error in allErrors)
                //{
                //    Console.WriteLine(error.ErrorMessage); 
                //}

                var user = await _userManager.GetUserAsync(User);

                var post = new PetPost
                {
                    Type = model.Type,
                    Title = model.Title,
                    Description = model.Description,
                    UserId = user.Id,
                    DateCreated = DateTime.Now,
                    DateLostFound = model.DateLostFound,
                    ChipNumber = model.ChipNumber,
                    Address = model.Address 
                };

                if (ImageFile != null)
                {
                    post.ImageUrl = await SaveImageAsync(ImageFile);
                }

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(model);
        }
        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var path = Path.Combine("wwwroot/images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return "/images/" + fileName;
        }
    }

}

    

