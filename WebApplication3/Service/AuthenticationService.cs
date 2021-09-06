using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication3.Data;
using WebApplication3.Dto.UserDto;
using WebApplication3.Models;

namespace WebApplication3.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly StoreContext context;
        private readonly IConfiguration configuration;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;
        private readonly IHttpContextAccessor httpContextAcccessor;

        public AuthenticationService(StoreContext context, IConfiguration configuration, IRefreshTokenGenerator refreshTokenGenerator, IHttpContextAccessor httpContextAcccessor)
        {
            this.context = context;
            this.configuration = configuration;
            this.refreshTokenGenerator = refreshTokenGenerator;
            this.httpContextAcccessor = httpContextAcccessor;
        }

        int GetUserId() => int.Parse(httpContextAcccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<int>> Register(RegisterUser obj)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            CreatePassword(obj.Password, out byte[] passwordhash, out byte[] passwordsalt);
            if (await UserExists(obj.UserName) == true)
            {
                response.Message = "Already Exists";
                response.Success = false;
                return response;
            }

            User user = new User()
            {
                UserName = obj.UserName,
                PasswordHash = passwordhash,
                PasswordSalt = passwordsalt
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            response.Data = user.UserId;
            return response;
        }

        private void CreatePassword(string password, out byte[] passwordhash, out byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private async Task<bool> UserExists(string username)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<ServiceResponse<AuthenticationResult>> Login(LoginUser obj)
        {
            ServiceResponse<AuthenticationResult> response = new ServiceResponse<AuthenticationResult>();
            User user = await context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == obj.UserName.ToLower());
            if (user == null)
            {
                response.Message = "Invalid Input";
                response.Success = false;
                return response;
            }
            if (VerifyPassword(obj.Password, user.PasswordHash, user.PasswordSalt) == true)
            {
                response.Data = CreateToken(user);
                RefreshToken token = await context.refreshTokens.FirstOrDefaultAsync(R => R.UserID == user.UserId);
                if (token == null)
                {
                    RefreshToken refreshToken = new RefreshToken()
                    {
                        UserID = user.UserId,
                        Token = response.Data.RefreshToken,
                        IsBlackListed = false,
                        User = user
                    };
                    await context.refreshTokens.AddAsync(refreshToken);
                    await context.SaveChangesAsync();
                }
                else
                {
                    token.Token = response.Data.RefreshToken;
                    context.refreshTokens.Update(token); ;
                    await context.SaveChangesAsync();
                }
                response.Message = "Found";
            }
            return response;
        }
        private bool VerifyPassword(string password, byte[] passwordhash, byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordsalt))
            {
                var computehash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computehash.Length; ++i)
                {
                    if (computehash[i] != passwordhash[i])
                        return false;
                }
                return true;
            }
        }


        private AuthenticationResult CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = cred,
                Expires = DateTime.UtcNow.AddSeconds(50)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            string refreshtoken = refreshTokenGenerator.GenerateToken();

            return new AuthenticationResult()
            {
                JwtToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshtoken
            };
        }

        private ServiceResponse<AuthenticationResult> CreateRefreshedToken(User user, Claim[] claims)
        {

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = cred,
                Expires = DateTime.UtcNow.AddSeconds(50)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            string refreshtoken = refreshTokenGenerator.GenerateToken();

            AuthenticationResult result = new AuthenticationResult()
            {
                JwtToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshtoken
            };

            ServiceResponse<AuthenticationResult> response = new ServiceResponse<AuthenticationResult>();
            response.Data = result;

            return response;
        }

        public async Task<ServiceResponse<AuthenticationResult>> RefreshToken(TokenToRefresh obj)
        {
            ServiceResponse<AuthenticationResult> response = new ServiceResponse<AuthenticationResult>();
            SecurityToken validationToken;
            var tokenhandler = new JwtSecurityTokenHandler();
            var principal = tokenhandler.ValidateToken(obj.JwtToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out validationToken);

            var Jwttoken = validationToken as JwtSecurityToken;

            var username = principal.Identity.Name;

            User user = await context.Users.FirstOrDefaultAsync(U => U.UserName.ToLower() == username.ToLower());

            RefreshToken orginaltoken = await context.refreshTokens.FirstOrDefaultAsync(R => R.UserID == user.UserId);

            if (orginaltoken.IsBlackListed == true)
            {
                throw new Exception("Login First");
            }

            if (obj.refreshToken != orginaltoken.Token)
            {
                throw new Exception("Tokens Do Not Match");
            }

            return (CreateRefreshedToken(user, principal.Claims.ToArray()));

        }

        public async Task<ServiceResponse<string>> Logout()
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = new User();

            user = await context.Users.FirstAsync(u => u.UserId == GetUserId());
            RefreshToken orginaltoken = await context.refreshTokens.FirstOrDefaultAsync(R => R.UserID == user.UserId);

            orginaltoken.IsBlackListed = true;
            context.refreshTokens.Update(orginaltoken);
            await context.SaveChangesAsync();
            response.Message = "You are logged out";
            return response;
        }
    }
}