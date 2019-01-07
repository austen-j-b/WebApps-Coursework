using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestingUsers.Models;
using WebApps.Data;
using WebApps.Models;
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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

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
