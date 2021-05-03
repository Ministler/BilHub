using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using backend.Models;
using backend.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;

namespace backend.Data.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<GetUserDto>> check()
        {
            ServiceResponse<GetUserDto> response = new ServiceResponse<GetUserDto>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            response.Data = _mapper.Map<GetUserDto>(user);
            return response;
        }

        public async Task<ServiceResponse<string>> Verify(UserVerifyDto userVerifyDto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(userVerifyDto.Email));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }
            if (!user.VerificationCode.Equals(userVerifyDto.Code))
            {
                response.Success = false;
                response.Message = "Code is not correct.";
                return response;
            }
            user.VerifiedStatus = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            response.Data = "Your account has been activated.";
            response.Message = "Your account has been activated.";

            return response;
        }
        public async Task<ServiceResponse<string>> ForgotMyPassword(UserForgotDto userForgotDto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(userForgotDto.Email.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }
            string randomPassword = Utility.GenerateRandomPassword();
            Utility.CreatePasswordWithSalt(randomPassword, user.PasswordSalt, out byte[] passwordHash);
            user.SecondPasswordHash = passwordHash;
            Utility.SendMail(user.Email, randomPassword, true);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            // EmailService emailService = new EmailService(emailCon);
            response.Data = "A recovery password has been sent to your mail, We recommend you to change it as soon as you receive it. By the meantime you can safely use your old password if you somehow you remember it.";
            response.Message = "A recovery password has been sent to your mail, We recommend you to change it as soon as you receive it. By the meantime you can safely use your old password if you somehow you remember it.";

            return response;
        }

        public async Task<ServiceResponse<string>> ChangePassword(UserChangeDto userChangeDto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(userChangeDto.Email.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(userChangeDto.Password, user.PasswordHash, user.PasswordSalt) && !VerifyPasswordHash(userChangeDto.Password, user.SecondPasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else
            {
                Utility.CreatePasswordHash(userChangeDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.SecondPasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                response.Data = "You password has succesfully been changed, you will be logged out.";
                response.Message = "You password has succesfully been changed, you will be logged out.";
            }
            return response;
        }

        public async Task<ServiceResponse<GetUserDto>> Login(UserLoginDto userLoginDto)
        {
            ServiceResponse<GetUserDto> response = new ServiceResponse<GetUserDto>();
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(userLoginDto.Email.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt) && !VerifyPasswordHash(userLoginDto.Password, user.SecondPasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else if (!user.VerifiedStatus)
            {
                response.Success = false;
                response.Message = "Please verify your account";
            }
            else
            {
                response.Data = _mapper.Map<GetUserDto>(user);
                response.Data.Token = CreateToken(user);
                response.Data.UserType = user.UserType;
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(UserRegisterDto userDto)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            User newUser = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(userDto.Email.ToLower()));
            if (newUser != null)
            {
                if (newUser.VerifiedStatus)
                {
                    response.Success = false;
                    response.Message = "User already exists.";
                    return response;
                }
            }
            if (newUser != null)
                _context.Users.Remove(newUser);
            newUser = new User { Email = userDto.Email, Name = userDto.Name };

            if (Utility.CheckIfInstructorEmail(userDto.Email))
            {
                newUser.UserType = UserTypeClass.Instructor;
            }
            else
            {
                newUser.UserType = UserTypeClass.Student;
            }

            Utility.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            newUser.PasswordHash = passwordHash;
            newUser.SecondPasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            string code = Utility.GenerateRandomCode();
            Utility.SendMail(newUser.Email, code, false);
            newUser.VerificationCode = code;
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            response.Data = newUser.Id;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Email.ToLower().Equals(username.ToLower())))
            {
                return true;
            }
            return false;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)
            );

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<ServiceResponse<string>> Resend(string email)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }
            user.VerificationCode = Utility.GenerateRandomCode();
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            Utility.SendMail(email, user.VerificationCode, false);
            response.Data = "new verification code has been sent to your email";
            return response;
        }

        public async Task<ServiceResponse<int>> IdOfUser(string email)
        {
            ServiceResponse<int> serviceResponse = new ServiceResponse<int> ();
            User dbUser = await _context.Users.FirstOrDefaultAsync ( x => x.Email.Equals ( email ) );
            if ( dbUser == null ) 
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "There is no such user";
                return serviceResponse;
            }

            serviceResponse.Data = dbUser.Id;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserInfoDto>> UpdateProfile(UserUpdateDto userUpdateDto)
        {
            ServiceResponse<GetUserInfoDto> serviceResponse = new ServiceResponse<GetUserInfoDto>();
            User dbUser = await _context.Users
                .FirstOrDefaultAsync ( c => c.Id == GetUserId() );
            
            if ( dbUser == null ) 
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found";
                return serviceResponse;
            }

            dbUser.Email = userUpdateDto.Email;
            dbUser.DarkModeStatus = userUpdateDto.DarkModeStatus;
            dbUser.Name = userUpdateDto.Name;
            dbUser.ProfileInfo = userUpdateDto.ProfileInfo;
            serviceResponse.Data = _mapper.Map<GetUserInfoDto> (dbUser);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserInfoDto>> GetProfile(int userId)
        {
            ServiceResponse<GetUserInfoDto> serviceResponse = new ServiceResponse<GetUserInfoDto>();
            User dbUser = await _context.Users
                .FirstOrDefaultAsync ( c => c.Id == userId );
            
            if ( dbUser == null ) 
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found";
                return serviceResponse;
            }
            
            serviceResponse.Data = _mapper.Map<GetUserInfoDto> (dbUser);
            return serviceResponse;
        }
    }
}