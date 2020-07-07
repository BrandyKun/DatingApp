using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Models/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                //create role

                var roles= new List<Role>
                {
                    new Role{Name = "Member"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Moderator"},
                    new Role{Name = "VIP"}
                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                   userManager.CreateAsync(user, "password").Wait();
                   userManager.AddToRoleAsync(user, "Member");
                }

                var adminUser = new User
                {
                    UserName = "Admin"
                };

                var result = userManager.CreateAsync(adminUser,"password").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});
                }
            }
        }

        private static void CreatePasswordHash(string passsword, out byte[] passwordHash, out byte[] passswordSalt)
        {
            //gets the password and store it in Hash and Salt form.
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passswordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passsword)); //gets the password in form of bytes
            }
            
        }
    }
}