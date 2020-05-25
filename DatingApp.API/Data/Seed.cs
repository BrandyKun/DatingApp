using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static void SeedUsers(DataContext context)
        {
            if (!context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Models/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    byte[] passwordhash, passwordSalt;
                    CreatePasswordHash("password",out passwordhash, out passwordSalt);
                    user.PasswordHash= passwordhash;
                    user.PasswordSalt = passwordSalt;
                    user.Username =user.Username.ToLower();
                    context.Users.Add(user);
                }
                context.SaveChanges();
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