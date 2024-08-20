using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LevelUp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LevelUp.App_Start
{
    public class RoleSeeder
    {
        public static void SeedRolesAndAdmin()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                if (!roleManager.RoleExists("Admin"))
                {
                    var role = new IdentityRole { Name = "Admin" };
                    roleManager.Create(role);
                }

                if (!roleManager.RoleExists("User"))
                {
                    var role = new IdentityRole { Name = "User" };
                    roleManager.Create(role);
                }

                var user = userManager.FindByEmail("admin@admin.com");
                if (user == null)
                {
                    user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };
                    var userResult = userManager.Create(user, "P@ssword1");

                    if (userResult.Succeeded)
                    {
                        userManager.AddToRole(user.Id, "Admin");
                    }
                }
            }
        }
    }
}