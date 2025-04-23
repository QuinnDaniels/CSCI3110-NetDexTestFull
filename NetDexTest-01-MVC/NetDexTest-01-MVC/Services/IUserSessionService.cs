namespace NetDexTest_01_MVC.Services
{
    /// <summary>
    /// class that provides a workaround for cookies to store the current user
    /// </summary>
    public interface IUserSessionService
    {
        void SetUserSession(string email, string accessToken, string refreshToken);
        void SetUserSession(string email, string accessToken, string refreshToken, List<string> roles);
        Task SetUserSessionAsync(string email, string accessToken, string refreshToken);
        Task SetUserSessionAsync(string email, string accessToken, string refreshToken, List<string> roles);
        Task SetUserSessionAsync(string email, string accessToken, string refreshToken, string rolesString);
        Task CloseUserSessionData();
        bool HasRole(string searchrole);
        Task<bool> HasRoleAsync(string searchrole);
        bool HasAnyRole(params string[] items);
        bool HasAnyRole(List<string> searchroles);
        Task<bool> HasAnyRoleAsync(List<string> searchrole);
        Task<bool> HasAnyRoleAsync(params string[] items);
        bool HasAllRoles(List<string> searchroles);
        bool HasAllRoles(params string[] items);
        Task<bool> HasAllRolesAsync(List<string> searchrole);
        Task<bool> HasAllRolesAsync(params string[] items);



        string GetEmail();
        string GetAccessToken();
        string GetRefreshToken();
        bool IsLoggedIn();
        string GetStringRoles();
        Task<string> GetStringRolesAsync();

        List<string> GetRoles();
        Task<List<string>> GetRolesAsync();

    }
}
