/*
 * 
 * https://github.com/alimozdemir/medium/blob/master/AuthJWTRefres
 * 
 * **/


using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NetDexTest_01
{
    public class JwtTokenCreator
    {
        private readonly JwtSettings _settings;

        public JwtTokenCreator(JwtSettings settings)
        {
            _settings = settings;
        }
        public string Generate(string email, string userId)
        {
            try
            {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _settings.Issuer,
                _settings.Audience,
                claims,
                expires: DateTime.UtcNow + _settings.Expire,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex) { Console.WriteLine($"\n\n\n\n\n\nEXCEPTION OCCURED:  {ex} \n\n\n\n\n");

                throw new Exception($"\n\n\n\n\n\nEXCEPTION OCCURED:  {ex} \n\n\n\n\n");

            }
        }
    }
}