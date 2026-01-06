using System.Security.Claims;
using Logex.API.Data;
using Logex.API.Models;
using Logex.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Logex.API.Services.Implementations
{
    public class UserManagement(
        UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        AppDbContext context
    ) : IUserManagement
    {
        public async Task<bool> CreateUser(User user, string role)
        {
            var _user = await GetUserByEmail(user.Email!);
            if (_user != null)
                return false;

            var result = await userManager.CreateAsync(user!, user!.PasswordHash!);
            if (!result.Succeeded)
                return false;

            bool roleExists = await roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(role));
            }

            await userManager.AddToRoleAsync(user, role);

            return true;
        }

        public async Task<IEnumerable<User?>> GetAllUsers() => await context.Users.ToListAsync();

        public async Task<User?> GetUserByEmail(string email) =>
            await userManager.FindByEmailAsync(email);

        public async Task<User> GetUserById(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            return user!;
        }

        public async Task<List<Claim>> GetUserClaims(string email)
        {
            var _user = await GetUserByEmail(email);

            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, _user!.Id.ToString()),
                new Claim(ClaimTypes.Name, _user!.UserName ?? ""),
                new Claim(ClaimTypes.Email, _user!.Email!),
            ];

            var roles = await userManager.GetRolesAsync(_user!);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public async Task<bool> LoginUser(User user)
        {
            var _user = await GetUserByEmail(user.Email!);
            if (_user is null)
            {
                return false;
            }

            return await userManager.CheckPasswordAsync(_user, user.PasswordHash!);
        }

        public async Task<int> RemoveUserByEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);

            context.Users.Remove(user!);
            return await context.SaveChangesAsync();
        }
    }
}
