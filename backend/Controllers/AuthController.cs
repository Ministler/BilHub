using System.Threading.Tasks;
using backend.Dtos.User;
using BilHub.Data.Auth;
using BilHub.Dtos;
using BilHub.Dtos.User;
using BilHub.Models;
using Microsoft.AspNetCore.Mvc;
namespace BilHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            ServiceResponse<int> response = await _authRepo.Register(
                new User { Email = request.Email }, request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            ServiceResponse<string> response = await _authRepo.Login(
                request.Email, request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            ServiceResponse<string> response = await _authRepo.ForgotMyPassword(email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(UserChangeDto request)
        {
            ServiceResponse<string> response = await _authRepo.ChangePassword(
                request.Email, request.Password, request.newPassword
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}