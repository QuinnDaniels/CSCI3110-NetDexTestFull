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
        Task SetUserSessionAsync(string email, string accessToken, string refreshToken, List<string> roles, string username);
        Task SetUserSessionAsync(string email, string accessToken, string refreshToken, string rolesString);
        Task SetUserSessionAsync(string email, string accessToken, string refreshToken, string rolesString, string username);
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

        Task SetTempCheckAsync(string email);
        Task CloseTempSessionData();

        Task StorePasswordSessionDataAsync(string pass);
        string GetPass();
        string GetEmail();
        string GetTempEmail();
        string GetUsername();
        string GetAccessToken();
        string GetRefreshToken();
        bool IsLoggedIn();
        string GetStringRoles();
        Task<string> GetStringRolesAsync();

        List<string> GetRoles();
        Task<List<string>> GetRolesAsync();

    }
}
