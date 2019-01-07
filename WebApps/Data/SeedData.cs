using WebApps.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace WebApps.Data
{
    public class SeedData
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
                var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

                IdentityResult roleResult;
                string testUserPW = "Password123!";

                var member1 = await EnsureUser(userManager, testUserPW, "Member1@email.com");
                await EnsureClaim(userManager, member1, "Member");

                var customer1 = await EnsureUser(userManager, testUserPW, "Customer1@email.com");
                await EnsureClaim(userManager, customer1, "Customer");

                var customer2 = await EnsureUser(userManager, testUserPW, "Customer2@email.com");
                await EnsureClaim(userManager, customer2, "Customer");

                var customer3 = await EnsureUser(userManager, testUserPW, "Customer3@email.com");
                await EnsureClaim(userManager, customer3, "Customer");

                var customer4 = await EnsureUser(userManager, testUserPW, "Customer4@email.com");
                await EnsureClaim(userManager, customer4, "Customer");

                var customer5 = await EnsureUser(userManager, testUserPW, "Customer5@email.com");
                await EnsureClaim(userManager, customer5, "Customer");
            }
        }

        private static async Task<string> EnsureUser(UserManager<IdentityUser> userManager, string testUserPw, string UserName)
        {
            IdentityUser user = await userManager.FindByNameAsync(UserName);

            if (user == null)
            {
                user = new IdentityUser { UserName = UserName };
                await userManager.CreateAsync(user, testUserPw);
            }

            return user.Id;
        }

        private static async Task<Boolean> EnsureClaim(UserManager<IdentityUser> userManager, string uid, string claim)
        {
            IdentityUser user = await userManager.FindByIdAsync(uid);

            var claimsList = (await userManager.GetClaimsAsync(user)).Select(p => p.Type);
            if (!claimsList.Contains(claim))
            {
                await userManager.AddClaimAsync(user, new Claim(claim, "true"));
            }
            return true;
        }

        private static async Task<IdentityResult> EnsureRole(RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager, string uid, string role)
        {
            IdentityResult IR = null;

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = await userManager.FindByIdAsync(uid);

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
    }
}
