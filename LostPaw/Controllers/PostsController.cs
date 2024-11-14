using Microsoft.AspNetCore.Mvc;
using LostPaw.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using LostPaw.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        public async Task<IActionResult> Create(PetPost post, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
              
                foreach (var error in allErrors)
                {
                    Console.WriteLine(error.ErrorMessage); 
                }

                var user = await _userManager.GetUserAsync(User);
                post.UserId = user.Id;
                post.DateCreated = DateTime.Now;

                if (ImageFile != null)
                {
                    post.ImageUrl = await SaveImageAsync(ImageFile);
                }

                if (post.Address != null)
                {
                    _context.Addresses.Add(post.Address);
                    await _context.SaveChangesAsync();
                    post.AddressId = post.Address.Id;
                }

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View("CreatePost", post);
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

    

