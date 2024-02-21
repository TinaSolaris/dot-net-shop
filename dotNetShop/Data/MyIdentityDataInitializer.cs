using Microsoft.AspNetCore.Identity;
using System;

namespace dotNetShop.Data
{
    public class MyIdentityDataInitializer
    {
        public static void SeedData(UserManager<IdentityUser> userManager,
                  RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        // name - correct email
        // password - min 8 charcters, small and capital letter, digit and special char
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = Enum.GetValues(typeof(Role));

            foreach (var role in roles)
            {
                string roleName = role.ToString();
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = roleName,
                    };
                    IdentityResult roleResult = roleManager.CreateAsync(identityRole).Result;
                }
            }
        }

        public static void SeedOneUser(
            UserManager<IdentityUser> userManager,
            string name, string password, Role? role = null)
        {
            if (userManager.FindByNameAsync(name).Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = name, // the same like the email
                    Email = name
                };

                IdentityResult result = userManager.CreateAsync(user, password).Result;
                if (result.Succeeded && role != null)
                {
                    userManager.AddToRoleAsync(user, role.ToString()).Wait();
                }
            }
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            SeedOneUser(userManager, "normaluser@localhost", "nUpass1!");
            SeedOneUser(userManager, Settings.AdminCreds.Username, Settings.AdminCreds.Password, Role.Admin);
        }
    }
}