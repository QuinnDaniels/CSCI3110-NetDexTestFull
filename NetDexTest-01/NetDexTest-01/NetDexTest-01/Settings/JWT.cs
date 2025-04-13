namespace NetDexTest_01.Settings
{
    /// <summary>
    ///  used to read data from our previously created JWT Section of appsettings.json using the IOptions feature of ASP.NET Core
    /// </summary>
    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInMinutes { get; set; }
    }
}
