using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApps.Models;
using WebApps.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApps.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        private string LoggedInUsername => User.Identity.Name;

        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {

            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.ToListAsync());
        }

        public async Task<Boolean> IsPostOwner(Post post)
        {
            var postOwner = await _userManager.FindByIdAsync(post.OwnerId);
            // currentUser will error if not logged in, this is fine as you will have to be logged in to do this action.
            var currentUser = await _userManager.FindByNameAsync(LoggedInUsername);

            if (postOwner != currentUser)
            {
                return false; // CREATE A PROPER "NOT OWNER OF POST" 
            }
            return true; 
        }

        private async Task<FullPostViewModel> GetFullPostVMFromPost(Post post)
        {
            FullPostViewModel viewModel = new FullPostViewModel();
            viewModel.Post = post;

            List<Comment> comments = await _context.Comments
                .Where(m => m.ParentPost == post).ToListAsync();

            viewModel.Comments = comments;

            viewModel.OwnerUserName = post.OwnerUsername;
            return viewModel;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post post = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            FullPostViewModel viewModel = await GetFullPostVMFromPost(post);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([Bind("PostID,Comment")] FullPostViewModel vm)
        {

            FullPostViewModel viewModel = null;
            if (ModelState.IsValid)
            {
                Comment newComment = new Comment();

                var Owner = await _userManager.FindByNameAsync(LoggedInUsername);
                newComment.CommentText = vm.Comment;
                newComment.DatePosted = DateTime.Now;
                newComment.OwnerId = Owner.Id;
                newComment.OwnerUserName = Owner.UserName; 

                Post post = await _context.Posts
                    .FirstOrDefaultAsync(m => m.PostId == vm.PostID);
                if (post == null)
                {
                    return NotFound();
                }

                newComment.ParentPost = post;
                post.NoOfComments++;
                _context.Add(newComment);
                await _context.SaveChangesAsync();

                viewModel = await GetFullPostVMFromPost(post);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [ActionName("Like")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int commentId, int postId)
        {
            Comment comment = await _context.Comments
                    .FirstOrDefaultAsync(m => m.CommentId == commentId);

            Like newLike = new Like();
            var Liker = await _userManager.FindByNameAsync(LoggedInUsername);
            newLike.LikerId = Liker.Id;

            Like existingLike = await _context.Likes
                .FirstOrDefaultAsync(m => (m.LikerId == newLike.LikerId && m.CommentId == commentId));
            newLike.CommentId = commentId;
            if(existingLike == null)
            {
                comment.NoOfLikes++;
                _context.Add(newLike);
                _context.SaveChanges();
                //await _context.SaveChangesAsync();
            } if(existingLike != null)
            {
                comment.NoOfLikes--;
                _context.Entry(existingLike).State = EntityState.Deleted;
                _context.Remove(existingLike);
                _context.SaveChanges();
                //await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", new { id = postId });
        }

        [Authorize(Policy = "IsAdminAccess")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsAdminAccess")]
        public async Task<IActionResult> Create([Bind("Title,PostText")] CreatePostViewModel vm)
        {
            Post post = new Post();
            if (ModelState.IsValid)
            {
                //post.PostId = 123; Dont need to pass ID as db autogenerates it
                post.NoOfComments = 0;
                post.OwnerId = _userManager.GetUserId(User);
                post.OwnerUsername = _userManager.GetUserName(User);
                post.DatePosted = DateTime.Now;
                post.PostText = vm.PostText;
                post.Title = vm.Title;

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
