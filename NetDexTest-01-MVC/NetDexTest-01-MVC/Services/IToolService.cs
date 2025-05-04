namespace NetDexTest_01.Services
{
    public interface IToolService
    {

        string Writer(string input);

        Task ConOut(string input);

        string Writer(Exception ex);

        Task ConOut(string input, Exception ex);

        /// <summary>
        /// <para>
        /// An extension method that truncates a string if they are greater than a specified integer in length
        /// </para>
        /// 
        /// <code>
        /// var s = "abcdefg";
        /// 
        /// Console.WriteLine(s.Truncate(3));
        /// </code>
        /// </summary>
        /// <remarks>
        ///     Source: <see href="https://stackoverflow.com/a/6724896">Truncate Extension Method</see>
        /// </remarks>
        /// <param name="value">The string to possibly truncate</param>
        /// <param name="maxChars">The specified number of characters that will be displayed before truncating</param>
        /// <returns></returns>
        //string Truncate(this string value, int maxChars);

    }
}


/// TODO - how to add xml docs to a namespace???
/// <summary>hi there</summary>
///
/// this was made to hold extension methods
namespace toolExtensions
{

    /// <summary>
    /// Extension class to aid in string manipulation
    /// <para>
    ///     Source: <see href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/how-to-implement-and-call-a-custom-extension-method">How to implement and call a custom extension method</see>
    /// </para>
    /// <code>
    /// using toolExtensions;
    ///
    /// </code>
    /// </summary>
    /// 
    /// <remarks>
    /// Extension methods must be defined in a static class.
    /// </remarks>
    public static class StringExtension
    {

        /// <summary>
        /// <para>
        /// An extension method that truncates a string if they are greater than a specified integer in length
        /// </para>
        /// 
        /// <code>
        /// using toolExtensions;
        /// 
        /// var s = "abcdefg";
        /// 
        /// Console.WriteLine(s.Truncate(3));
        /// </code>
        /// </summary>
        /// <remarks>
        ///     Source: <see href="https://stackoverflow.com/a/6724896">Stack Overflow: Truncate Extension Method</see>
        /// </remarks>
        /// <param name="value">The string to possibly truncate</param>
        /// <param name="maxChars">The specified number of characters that will be displayed before truncating</param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }


        public static bool IsNullOrEmpty(this string? value)
        {
            if (value == null) return true;
            return string.IsNullOrEmpty(value);
        }
    }

}
