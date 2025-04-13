/**
 *
 * https://code-maze.com/using-refresh-tokens-in-asp-net-core-authentication/
 */


using System.Security.Claims;

namespace NetDexTest_01.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// contains the logic to generate the access token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        string GenerateAccessToken(IEnumerable<Claim> claims);
        /// <summary>
        ///  contains the logic to generate the refresh token. We use the <see cref="System.Security.Cryptography.RandomNumberGenerator">RandomNumberGenerator</see> class to generate a cryptographic random number for this purpose.
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();
        /// <summary>
        /// used to get the user principal from the expired access token.
        /// We make use of the <see cref="System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.ValidateToken(string, Microsoft.IdentityModel.Tokens.TokenValidationParameters, out Microsoft.IdentityModel.Tokens.SecurityToken)">ValidateToken()</see> method 
        /// of <see cref="System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.JwtSecurityTokenHandler">JwtSecurityTokenHandler</see> class for this purpose.
        /// This method validates the token and returns the <see cref="System.Security.Claims.ClaimsPrincipal">ClaimsPrincipal</see> object.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
