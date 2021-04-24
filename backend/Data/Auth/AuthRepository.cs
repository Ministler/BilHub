using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BilHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BilHub.Data.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ServiceResponse<string>> ForgotMyPassword(string email)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }
            string randomPassword = GenerateRandomPassword();
            Utility.CreatePasswordWithSalt(randomPassword, user.PasswordSalt, out byte[] passwordHash);
            user.SecondPasswordHash = passwordHash;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            response.Data = "A recovery password has been sent to your mail, We recommend you to change it as soon as you receive it. By the meantime you can safely use your old password if you somehow you remember it.";
            response.Message = "A recovery password has been sent to your mail, We recommend you to change it as soon as you receive it. By the meantime you can safely use your old password if you somehow you remember it.";

            return response;
        }

        public async Task<ServiceResponse<string>> ChangePassword(string email, string password, string newPassword)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) && !VerifyPasswordHash(password, user.SecondPasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else
            {
                Utility.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
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

        private string GenerateRandomPassword()
        {
            string ret = "";
            Random random = new Random();
            int len = random.Next() % 8 + 9;
            for (int i = 0; i < len; i++)
            {
                char tmp = (char)('a' + (random.Next() % 26));
                char tmp2 = (char)('1' + (random.Next() % 9));
                char tmp3 = '_';
                char tmp4 = 'a';
                int tmp5 = random.Next() % 4;
                if (tmp5 == 0)
                    tmp4 = tmp;
                if (tmp5 == 1)
                    tmp4 = tmp2;
                if (tmp5 == 2)
                    tmp4 = tmp3;
                if (tmp5 == 3)
                    tmp4 = (char)('A' + tmp - 'a');
                ret = ret + tmp4.ToString();
            }
            return ret;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) && !VerifyPasswordHash(password, user.SecondPasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (await UserExists(user.Email))
            {
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }

            Utility.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.SecondPasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Email.ToLower() == username.ToLower()))
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
    }
}