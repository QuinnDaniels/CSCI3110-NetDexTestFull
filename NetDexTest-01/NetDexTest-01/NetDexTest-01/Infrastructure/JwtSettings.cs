/*
 * 
 * https://github.com/alimozdemir/medium/blob/master/AuthJWTRefres
 * 
 * **/

using System;

namespace NetDexTest_01
{
    /// <summary>
    /// Values are set in appsettings.json
    /// </summary>
    public class JwtSettings
    {

        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int MinutesToExpiration { get; set; }
        /// <summary>
        /// References MinutesToExpiration
        /// </summary>
        public TimeSpan Expire => TimeSpan.FromMinutes(MinutesToExpiration);
    }
}