using System.Threading.Tasks;
using BilHub.Models;

namespace BilHub.Data.Auth
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<string>> ForgotMyPassword(string email);
        Task<ServiceResponse<string>> ChangePassword(string username, string password, string newPassword);
        Task<bool> UserExists(string username);
    }
}