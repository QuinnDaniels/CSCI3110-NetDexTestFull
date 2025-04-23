namespace NetDexTest_01_MVC.Services
{

    /// <inheritdoc cref="IUserSessionService"/>
    public class UserSessionService : IUserSessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetUserSession(string email, string accessToken, string refreshToken)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("Email", email);
            Console.WriteLine($"\n\n{session.GetString("Email")}");
            session.SetString("AccessToken", accessToken);
            Console.WriteLine($"\n\n{session.GetString("AccessToken")}");
            session.SetString("RefreshToken", refreshToken);
            Console.WriteLine($"\n\n{session.GetString("RefreshToken")}\n\n\n");
        }
        public void SetUserSession(string email, string accessToken, string refreshToken, List<string> roles)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("Email", email);
            Console.WriteLine($"\n\n{session.GetString("Email")}");
            session.SetString("AccessToken", accessToken);
            Console.WriteLine($"\n\n{session.GetString("AccessToken")}");
            session.SetString("RefreshToken", refreshToken);
            Console.WriteLine($"\n\n{session.GetString("RefreshToken")}\n\n\n");
            string roleString = roles.Aggregate("", (current, s) => current + (s + ","));
            session.SetString("StringRoles", roleString);
            Console.WriteLine($"\n\nRoles:\t{session.GetString("StringRoles")}");

        }

        public async Task SetUserSessionAsync(string email, string accessToken, string refreshToken)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("Email", email);
            session.SetString("AccessToken", accessToken);
            session.SetString("RefreshToken", refreshToken);
            await Console.Out.WriteLineAsync($"\n\nEmail:\t{session.GetString("Email")}");
            await Console.Out.WriteLineAsync($"\n\nAccessToken:\t{session.GetString("AccessToken")}");
            await Console.Out.WriteLineAsync($"\n\nRefreshToken:\t{session.GetString("RefreshToken")}\n\n\n");
            
        }
        public async Task SetUserSessionAsync(string email, string accessToken, string refreshToken, List<string> roles)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("Email", email);
            session.SetString("AccessToken", accessToken);
            session.SetString("RefreshToken", refreshToken);

            string roleString = roles.Aggregate("", (current, s) => current + (s + ","));
            session.SetString("StringRoles", roleString);
            await Console.Out.WriteLineAsync($"\n\nRoles:\t{session.GetString("StringRoles")}");
            await Console.Out.WriteLineAsync($"\n\nEmail:\t{session.GetString("Email")}");
            await Console.Out.WriteLineAsync($"\n\nAccessToken:\t{session.GetString("AccessToken")}");
            await Console.Out.WriteLineAsync($"\n\nRefreshToken:\t{session.GetString("RefreshToken")}\n\n\n");
            
        }

        public async Task SetUserSessionAsync(string email, string accessToken, string refreshToken, string rolesString)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            await ConOut("Setting User Session....");
            
            session.SetString("Email", email);
            session.SetString("AccessToken", accessToken);
            session.SetString("RefreshToken", refreshToken);

            session.SetString("StringRoles", rolesString);

            await Console.Out.WriteLineAsync($"\n\nRoles:\t{session.GetString("StringRoles")}");
            await Console.Out.WriteLineAsync($"\n\nEmail:\t{session.GetString("Email")}");
            await Console.Out.WriteLineAsync($"\n\nAccessToken:\t{session.GetString("AccessToken")}");
            await Console.Out.WriteLineAsync($"\n\nRefreshToken:\t{session.GetString("RefreshToken")}\n\n\n");

        }

        public async Task CloseUserSessionData()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            await ConOut("Logout - Closing User Session....");
            await Console.Out.WriteLineAsync($"\n\nSetting Session Strings to null...");
            session.SetString("Email", "");
            session.SetString("AccessToken", "");
            session.SetString("RefreshToken", "");
            
            await Console.Out.WriteLineAsync($"\n\nClearing Session...");
            session.Clear();

            await Console.Out.WriteLineAsync($"\n\nEmail:\t{session.GetString("Email")}");
            await Console.Out.WriteLineAsync($"\n\nAccessToken:\t{session.GetString("AccessToken")}");
            await Console.Out.WriteLineAsync($"\n\nRefreshToken:\t{session.GetString("RefreshToken")}\n\n\n");

        }

        private async Task MakeRequest()
        {
            // do some stuff
            //var task = Task.Run(() => Calculate(myInput));
            // do other stuff
            //var myOutput = await task;
            // some more stuff
        }
        public async Task<List<string>> GetRolesAsync()
        {
            // do some stuff
            var task = Task.Run(() => GetRoles());
            // do other stuff
            var myOutput = await task;
            // some more stuff
            return myOutput;
        }
        public async Task<string> GetStringRolesAsync()
        {
            // do some stuff
            var task = Task.Run(() => GetStringRoles());
            // do other stuff
            var myOutput = await task;
            // some more stuff
            return myOutput;
        }

        public bool HasRole(string searchrole)
        {
            var list = GetRoles();
            return list.Contains(searchrole);
        }

        public async Task<bool> HasRoleAsync(string searchrole)
        {
            return await Task.Run(() => HasRole(searchrole));
        }





        public bool HasAnyRole(List<string> searchroles)
        {
            
            var list = GetRoles();

            bool flagCheck = false;

            flagCheck = ContainsAny(searchroles.ToArray());
            return flagCheck;
        }        
        public async Task<bool> HasAnyRoleAsync(List<string> searchrole)
        {
            return await Task.Run(() => HasAnyRole(searchrole));
        }
        
        public bool HasAllRoles(List<string> searchroles)
        {
            var list = GetRoles();
            bool flagCheck = false;
            flagCheck = ContainsAll(searchroles.ToArray());
            return flagCheck;
        }
        public async Task<bool> HasAllRolesAsync(List<string> searchrole)
        {
            return await Task.Run(() => HasAllRoles(searchrole));
        }

        public bool HasAnyRole(params string[] items)
        {

            var list = GetRoles();

            bool flagCheck = false;

            flagCheck = ContainsAny(items);
            return flagCheck;
        }
        public async Task<bool> HasAnyRoleAsync(params string[] items)
        {
            return await Task.Run(() => HasAnyRole(items));
        }

        public bool HasAllRoles(params string[] items)
        {
            var list = GetRoles();
            bool flagCheck = false;
            flagCheck = ContainsAll(items);
            return flagCheck;
        }
        public async Task<bool> HasAllRolesAsync(params string[] items)
        {
            return await Task.Run(() => HasAllRoles(items));
        }

        private bool ContainsAll(params string[] items)
            => items.Any(i => !GetRoles().Contains(i));
        private bool ContainsAny(params string[] items)
            => items.Any(i => GetRoles().Contains(i));



        public string GetEmail() => _httpContextAccessor.HttpContext.Session.GetString("Email");
        public string GetStringRoles() => _httpContextAccessor.HttpContext.Session.GetString("StringRoles");
        
        public List<string> GetRoles()
        {
            var feeder = GetStringRoles();
            if (feeder == null) feeder = "";
            string[] words = feeder.Split('+');

            List<string> rolesOut = [.. words];

            rolesOut.RemoveAt(rolesOut.IndexOf(rolesOut.Last()));

            return rolesOut;
        }
        
        public string GetAccessToken() => _httpContextAccessor.HttpContext.Session.GetString("AccessToken");
        public string GetRefreshToken() => _httpContextAccessor.HttpContext.Session.GetString("RefreshToken");

        public bool IsLoggedIn() => !string.IsNullOrEmpty(GetAccessToken());


        private string Writer(string input)
        {
            string output = "";
            output =
                  "\n\n"
                + "--------------------"
                + "\n\n"
                + $"{input}"
                + "\n\n\n"
                + "-------------------"
                + "\n\n" ;
            return output;
        }

        private async Task ConOut(string input)
        {
            await Console.Out.WriteLineAsync($"{Writer(input)}");
        }
    }
}

