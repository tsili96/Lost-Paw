using Microsoft.AspNetCore.Mvc;
using LostPaw.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using LostPaw.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using LostPaw.ViewModels;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Index(string searchTerm, string postType, string dateFilter)
        {
            var postsQuery = _context.Posts
                .Include(p => p.User)
                .Select(post => new DisplayPostListViewModel
                {
                    Id = post.Id,
                    Type = post.Type,
                    Title = post.Title,
                    DateLostFound = post.DateLostFound,
                    ImageUrl = post.ImageUrl,
                    Username = post.User.UserName,
                    UserId = post.User.Id
                });

            if (!string.IsNullOrEmpty(searchTerm))
            {
                postsQuery = postsQuery.Where(p => p.Title.ToLower().Contains(searchTerm.ToLower()) || p.Username.ToLower() == searchTerm.ToLower());
            }

            //post type filter
            if (!string.IsNullOrEmpty(postType))
            {
                if (Enum.TryParse<PostType>(postType, ignoreCase: true, out var type))
                {
                    postsQuery = postsQuery.Where(p => p.Type == type);
                }
            }

            // date filter 
            if (!string.IsNullOrEmpty(dateFilter))
            {
                if (dateFilter == "mostRecent")
                {
                    postsQuery = postsQuery.OrderByDescending(p => p.DateLostFound);
                }
                else if (dateFilter == "oldest")
                {
                    postsQuery = postsQuery.OrderBy(p => p.DateLostFound);
                }
            }

            var posts = postsQuery.ToList();

            ViewData["NoResults"] = !posts.Any();
            ViewData["PostType"] = postType;
            ViewData["DateFilter"] = dateFilter;

            if (!posts.Any())
            {
                ViewData["NoResults"] = true;
            }

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
            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var folderPath = Path.Combine("wwwroot", "images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var path = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return "/images/" + fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
                return null; 
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts
                .Where(p => p.Id == id)
                .Include(p => p.Address)  // Make sure the Address is included
                .FirstOrDefaultAsync();

            if (post == null)
            {
                return NotFound();
            }

            var model = new UpdatePostViewModel
            {
                Id = post.Id,
                Type = post.Type,
                Title = post.Title,
                Description = post.Description,
                ChipNumber = post.ChipNumber,
                DateLostFound = post.DateLostFound,
                Address = post.Address // Make sure the Address is set
            };

            ViewData["CurrentImageUrl"] = post.ImageUrl; // Set current image URL

            return View("EditPost",model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = await _context.Posts.FindAsync(model.Id);
                if (post == null)
                {
                    return NotFound();
                }

                post.Type = model.Type;
                post.Title = model.Title;
                post.Description = model.Description;
                post.ChipNumber = model.ChipNumber;
                post.DateLostFound = model.DateLostFound;
                post.Address = model.Address;

                if (model.ImageFile != null)
                {
                    post.ImageUrl = await SaveImageAsync(model.ImageFile);
                }

                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            var existingPost = await _context.Posts.FindAsync(model.Id);
            ViewData["CurrentImageUrl"] = existingPost?.ImageUrl;

            return View(model);
        }


        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var post = _context.Posts
                .Include(p => p.User)
                .Select(p => new DisplayPostViewModel
                {
                    Id = p.Id,
                    Type = p.Type,
                    Title = p.Title,
                    Description = p.Description,
                    ChipNumber = p.ChipNumber,
                    DateLostFound = p.DateLostFound,
                    ImageUrl = p.ImageUrl,
                    Address = p.Address ?? new Address(),
                    Username = p.User.UserName,
                    UserId = p.User.Id
                })
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View("PostDetails", post);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.Include(p => p.Address).FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (post.UserId != currentUserId)
            {
                return Forbid();  
            }

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (post.UserId != currentUserId)
            {
                return Forbid(); 
            }

            // Delete post
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }

}

    

