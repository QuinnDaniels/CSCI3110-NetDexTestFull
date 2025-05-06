using System.ComponentModel.DataAnnotations;

namespace NetDexTest_01_MVC.Services
{

    public class UserSessionRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }


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

        public async Task SetUserSessionAsync(string email, string accessToken, string refreshToken, List<string> roles, string Username)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("Email", email);
            session.SetString("Username", Username);
            session.SetString("AccessToken", accessToken);
            session.SetString("RefreshToken", refreshToken);

            string roleString = roles.Aggregate("", (current, s) => current + (s + ","));
            session.SetString("StringRoles", roleString);
            await Console.Out.WriteLineAsync($"\n\nRoles:\t{session.GetString("StringRoles")}");
            await Console.Out.WriteLineAsync($"\n\nEmail:\t{session.GetString("Email")}");
            await Console.Out.WriteLineAsync($"\n\nUsername:\t{session.GetString("Username")}");
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

        public async Task SetUserSessionAsync(string email, string accessToken, string refreshToken, string rolesString, string Username)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            await ConOut("Setting User Session....");
            
            session.SetString("Email", email);
            session.SetString("AccessToken", accessToken);
            session.SetString("RefreshToken", refreshToken);
            session.SetString("Username", Username);


            session.SetString("StringRoles", rolesString);

            await Console.Out.WriteLineAsync($"\n\nRoles:\t{session.GetString("StringRoles")}");
            await Console.Out.WriteLineAsync($"\n\nEmail:\t{session.GetString("Email")}");
            await Console.Out.WriteLineAsync($"\n\nUsername\t{session.GetString("Username")}");
            await Console.Out.WriteLineAsync($"\n\nAccessToken:\t{session.GetString("AccessToken")}");
            await Console.Out.WriteLineAsync($"\n\nRefreshToken:\t{session.GetString("RefreshToken")}\n\n\n");

        }

        public async Task CloseUserSessionData()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            await ConOut("Logout - Closing User Session....");
            await Console.Out.WriteLineAsync($"\n\nSetting Session Strings to null...");
            session.SetString("Email", "");
            session.SetString("Username", "");
            session.SetString("AccessToken", "");
            session.SetString("RefreshToken", "");
            session.SetString("Pass", "");
            session.SetString("TempEmail", "");
            session.SetString("TempPerson", "");
            
            await Console.Out.WriteLineAsync($"\n\nClearing Session...");
            session.Clear();

            await Console.Out.WriteLineAsync($"\n\nEmail:\t{session.GetString("Email")}");
            await Console.Out.WriteLineAsync($"\n\nTempPerson:\t{session.GetString("TempPerson")}");
            await Console.Out.WriteLineAsync($"\n\nTempEmail:\t{session.GetString("TempEmail")}");
            await Console.Out.WriteLineAsync($"\n\nUsername:\t{session.GetString("Username")}");
            await Console.Out.WriteLineAsync($"\n\nPassword:\t{session.GetString("Pass")}");
            await Console.Out.WriteLineAsync($"\n\nAccessToken:\t{session.GetString("AccessToken")}");
            await Console.Out.WriteLineAsync($"\n\nRefreshToken:\t{session.GetString("RefreshToken")}\n\n\n");

        }

        public async Task StorePasswordSessionDataAsync(string pass)
        {
            await ConOut("\n\nStoring password...\n\n");
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("Pass", pass);
        }

        public async Task StoreCurrentUserDataAsync(string uName)
        {
            await ConOut("\n\nStoring username...\n\n");
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("Username", uName);
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



        public string GetPass() => _httpContextAccessor.HttpContext.Session.GetString("Pass");
        public string GetEmail() => _httpContextAccessor.HttpContext.Session.GetString("Email");
        public string GetTempEmail() => _httpContextAccessor.HttpContext.Session.GetString("TempEmail");
        public string GetTempPerson() => _httpContextAccessor.HttpContext.Session.GetString("TempPerson");
        public string GetTempRecordCollector() => _httpContextAccessor.HttpContext.Session.GetString("TempRecordCollector");
        public string GetTempContactInfo() => _httpContextAccessor.HttpContext.Session.GetString("TempContactInfo");
        public Int64 GetTempRecordCollectorId() => Int64.Parse(GetTempRecordCollector());// ?? "-1");
        public Int64 GetTempContactInfoId() => Int64.Parse(GetTempContactInfo());// ?? "-1");
        public string GetUsername() => _httpContextAccessor.HttpContext.Session.GetString("Username");
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



        // public bool IsSameUser() 


        public async Task SetTempCheckAsync(//string Username,
            //string accessToken, string refreshToken,
            //string rolesString,
            string email)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            await ConOut("Setting Temp User Session....");

            session.SetString("TempEmail", email);
            //session.SetString("TempAccessToken", accessToken);
            //session.SetString("TempRefreshToken", refreshToken);
            //session.SetString("TempUsername", Username);


            //session.SetString("TempStringRoles", rolesString);

            //await Console.Out.WriteLineAsync($"\n\nTempRoles:\t{session.GetString("TempStringRoles")}");
            await Console.Out.WriteLineAsync($"\n\nTempEmail:\t{session.GetString("TempEmail")}");
            //await Console.Out.WriteLineAsync($"\n\nTempUsername:\t{session.GetString("TempUsername")}");
            //await Console.Out.WriteLineAsync($"\n\nTempAccessToken:\t{session.GetString("TempAccessToken")}");
            //await Console.Out.WriteLineAsync($"\n\nTempRefreshToken:\t{session.GetString("TempRefreshToken")}\n\n\n");

        }

        /// <summary>
        /// Store user input and criteria (person nickname and id)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task SetTempCheckAsync(string email, string criteria)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            await ConOut("Setting Temp User Session....");

            session.SetString("TempEmail", email);
            session.SetString("TempPerson", criteria);
            await Console.Out.WriteLineAsync($"\n\nTempEmail:\t{session.GetString("TempEmail")}");
            await Console.Out.WriteLineAsync($"\n\nTempPerson:\t{session.GetString("TempPerson")}");
        }


        /// <summary>
        /// Store user input and criteria (person nickname and id)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task SetTempPersonAsync(string criteria)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            await ConOut("Setting Temp User Session....");

            session.SetString("TempPerson", criteria);
            await Console.Out.WriteLineAsync($"\n\nTempPerson:\t{session.GetString("TempPerson")}");
        }



        /// <summary>
        /// Store person's recordcollectorid and ContactInfoid (person nickname and id)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task SetTempPersonAsync(Int64 recordCollectorId, Int64 entryItemId)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            await ConOut("Setting Temp User Session....");

            session.SetString("TempRecordCollector", recordCollectorId.ToString());
            await Console.Out.WriteLineAsync($"\n\nTempRecordCollector:\t{session.GetString("TempRecordCollector")}");
            session.SetString("TempContactInfo", entryItemId.ToString());
            await Console.Out.WriteLineAsync($"\n\nTempContactInfo:\t{session.GetString("TempContactInfo")}");
        }




        //TODO - temp entry or Ientry service






        public async Task CloseTempSessionData()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            await ConOut("Closing Temp Session....");
            await Console.Out.WriteLineAsync($"\n\nSetting Session Strings to empty...");
            session.SetString("TempEmail", "");
            session.SetString("TempPerson", "");
            session.SetString("TempRecordCollector", "");
            session.SetString("TempContactInfo", "");


            await Console.Out.WriteLineAsync($"\n\nClearing Temp Session...");
            session.Remove("TempEmail");
            session.Remove("TempPerson");
            session.Remove("TempRecordCollector");
            session.Remove("TempContactInfo");



            await Console.Out.WriteLineAsync($"\n\nTempEmail:\t{session.GetString("TempEmail")}");
            await Console.Out.WriteLineAsync($"\n\nTempPerson:\t{session.GetString("TempPerson")}");
            await Console.Out.WriteLineAsync($"\n\nTempRecordCollector:\t{session.GetString("TempRecordCollector")}");
            await Console.Out.WriteLineAsync($"\n\nTempContactInfo:\t{session.GetString("TempContactInfo")}");

        }





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

