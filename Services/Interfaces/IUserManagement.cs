using System.Security.Claims;
using Logex.API.Models;

namespace Logex.API.Services.Interfaces
{
    public interface IUserManagement
    {
        Task<bool> CreateUser(User user, string role);
        Task<bool> LoginUser(User user);
        Task<User?> GetUserByEmail(string email);
        Task<User> GetUserById(int id);
        Task<IEnumerable<User?>> GetAllUsers();
        Task<int> RemoveUserByEmail(string email);
        Task<List<Claim>> GetUserClaims(string email);
    }
}
