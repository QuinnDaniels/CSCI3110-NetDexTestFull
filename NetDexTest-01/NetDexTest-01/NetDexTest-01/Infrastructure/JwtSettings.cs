/*
 * 
 * https://github.com/alimozdemir/medium/blob/master/AuthJWTRefres
 * 
 * **/

using System;

namespace NetDexTest_01
{
    public class JwtSettings
    {

        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int MinutesToExpiration { get; set; }

        public TimeSpan Expire => TimeSpan.FromMinutes(MinutesToExpiration);
    }
}