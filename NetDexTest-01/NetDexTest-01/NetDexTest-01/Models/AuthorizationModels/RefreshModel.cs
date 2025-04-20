using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/**
 * 
 * https://code-maze.com/authentication-aspnetcore-jwt-1/
 * 
 * 
 * https://memorycrypt.hashnode.dev/create-a-web-api-with-jwt-authentication-and-aspnet-core-identity
 */
namespace NetDexTest_01.Models
{
    public class RefreshModel
    {
        [Required]
        public string? Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
