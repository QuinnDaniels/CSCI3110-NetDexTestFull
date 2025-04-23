using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NetDexTest_01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace NetDexTest_01.Controllers
{

    public partial class AuthController
    {


        private async Task<ApplicationUser> AddUser(RegisterModel registerModel)
        {
            var newUser = await _userRepo.CreateUserDexHolderAsync(registerModel);
            return newUser;
        }
        private async Task<IdentityResult> AddUser(RegisterModel registerModel, bool tf)
        {
            var result = await _userRepo.CreateUserDexHolderAsync(registerModel, tf);
            return (IdentityResult)result;
        }
        /*-------------------------------*/
        private async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            return await _userRepo.GetUserAsync(PropertyField.email, email);
        }

        private async Task<ApplicationUser?> GetUser(string email, string password)
        {
            return await _userRepo.GetUserAsync(PropertyField.email, email, password);
        }


        private IActionResult BadRequestErrorMessages()
        {
            var errMsgs = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(errMsgs);
        }




        private async Task<AuthenticatedResponse> GetTokens(ApplicationUser user)
        {
            await Console.Out.WriteAsync($"\n\n\n\n----------Get Tokens------------\n\n\n");
            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtSettings:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                        //new Claim("DexHolderId", user.DexHolder.Id)
                    };
            await Console.Out.WriteAsync($"\nclaims:\n\t\t{claims}\n\n");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            await Console.Out.WriteAsync($"\nkey:\n\t\t{key}\n\n");
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            await Console.Out.WriteAsync($"\nsignIn:\n\t\t{signIn}\n\n");
            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:MinutesToExpiration"])),
                signingCredentials: signIn);
            
            await Console.Out.WriteAsync($"\ntoken:\n\t\t{token}\n\n");
            
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            await Console.Out.WriteAsync($"\ntokenStr:\n\t\t{tokenStr}\n\n");

            var refreshTokenStr = GetRefreshToken();
            await Console.Out.WriteAsync($"\nrefreshTokenStr:\n\t\t{refreshTokenStr}\n\n");
            user.RefreshToken = refreshTokenStr;
            var authResponse = new AuthResponse { Token = tokenStr, RefreshToken = refreshTokenStr };
            await Console.Out.WriteAsync($"\nauthResponse:\n\t\t{authResponse}\n\n");

            await Console.Out.WriteAsync($"\n\n\n\n-----END--Get Tokens------------\n\n\n");

            return await Task.FromResult(authResponse);
        }
        /*------------------------*/
        private string GetRefreshToken()
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            // ensure token is unique by checking against db
            var tokenIsUnique = !_userManager.Users.Any(u => u.RefreshToken == token);
            //var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == request.RefreshToken);



            if (!tokenIsUnique)
                return GetRefreshToken();  //recursive call

            return token;
        }
        private async Task<ApplicationUser> GetUserByRefreshToken(string refreshToken)
        {
            return await Task.FromResult(_userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken));
            //return await Task.FromResult(tempUserDb.FirstOrDefault(u => u.RefreshToken == refreshToken));
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])),
                ValidateLifetime = false //we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }



    }
}
