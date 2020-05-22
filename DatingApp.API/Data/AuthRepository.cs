using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            this._context = context;

        }
        public async Task<User> Login(string username, string passsword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if ( user == null)
                return null;
        
            if(!VerifyPasswordHash(passsword, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string passsword, byte[] passwordHash, byte[] passwordSalt)
        {
            //gets the password and store it in Hash and Salt form.
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passsword)); //gets the password in form of bytes
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            
            return true;
        }

        public async Task<User> Register(User user, string passsword)
        {
            byte[] passwordHash, passswordSalt;
            CreatePasswordHash(passsword, out passwordHash, out passswordSalt);
            
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passswordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string passsword, out byte[] passwordHash, out byte[] passswordSalt)
        {
            //gets the password and store it in Hash and Salt form.
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passswordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passsword)); //gets the password in form of bytes
            }
            
        }

        public async Task<bool> USerExists(string username)
        {
            if (await _context.Users.AnyAsync( x => x.Username == username))
                return true;
                
            return false;
        }
    }
}