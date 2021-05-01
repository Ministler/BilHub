using System.Threading.Tasks;
using backend.Data.Auth;
using backend.Dtos.User;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace backend.Controllers
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
            ServiceResponse<int> response = await _authRepo.Register(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            ServiceResponse<GetUserDto> response = await _authRepo.Login(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [Authorize]
        [HttpGet("Check")]
        public async Task<IActionResult> Check()
        {
            ServiceResponse<GetUserDto> response = await _authRepo.check();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(UserForgotDto request)
        {
            ServiceResponse<string> response = await _authRepo.ForgotMyPassword(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Verify")]
        public async Task<IActionResult> Verify(UserVerifyDto request)
        {
            ServiceResponse<string> response = await _authRepo.Verify(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Resend")]
        public async Task<IActionResult> Resend(string email)
        {
            ServiceResponse<string> response = await _authRepo.Resend(email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(UserChangeDto request)
        {
            ServiceResponse<string> response = await _authRepo.ChangePassword(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}