using System.Threading.Tasks;
using backend.Dtos.User;
using backend.Models;

namespace backend.Data.Auth
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(UserRegisterDto userDto);
        Task<ServiceResponse<string>> Login(UserLoginDto userLoginDto);
        Task<ServiceResponse<string>> ForgotMyPassword(UserForgotDto userForgotDto);
        Task<ServiceResponse<string>> ChangePassword(UserChangeDto userChangeDto);
        public Task<ServiceResponse<string>> Verify(UserVerifyDto userVerifyDto);
        Task<bool> UserExists(string username);
    }
}