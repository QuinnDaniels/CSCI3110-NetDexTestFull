/**
 * 
 * https://code-maze.com/authentication-aspnetcore-jwt-1/
 * 
 * 
 * https://memorycrypt.hashnode.dev/create-a-web-api-with-jwt-authentication-and-aspnet-core-identity
 */
namespace NetDexTest_01.Models
{
    public class AuthenticatedResponse
    {
        /// <summary>
        /// AccessToken
        /// </summary>
        public string? Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
