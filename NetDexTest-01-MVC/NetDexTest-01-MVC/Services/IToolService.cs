namespace NetDexTest_01_MVC.Services { 
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



        /// <summary>
        /// <para>
        /// An easy way to check if an array or list contains any of a number of provided strings. 
        /// </para>
        /// <para>
        /// An alternative to .Contains().
        /// </para>
        /// <code>
        /// if (title.EqualsAny("User greeting", "User name"))
        ///{
        ///    //do stuff
        ///}
        /// </code>
        /// </summary>
        /// <remarks>
        /// Source: 
        /// <see href="https://stackoverflow.com/a/41098725">Deilan - How to check if something equals any of a list of values in C#</see>
        /// </remarks>
        /// <param name="target"></param>
        /// <param name="comparer"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool EqualsAny(this string target, StringComparer comparer, params string[] values)
        {
            return target.EqualsAny(comparer, (IEnumerable<string>)values);
        }

        /// <summary>
        /// <inheritdoc cref="EqualsAny(string, StringComparer, string[])"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool EqualsAny(this string target, params string[] values)
        {
            return target.EqualsAny((IEnumerable<string>)values);
        }

        /// <inheritdoc cref="EqualsAny(string, StringComparer, string[])"/>
        public static bool EqualsAny(this string target, StringComparer comparer, IEnumerable<string> values)
        {
            return values.Contains(target, comparer);
        }

        /// <inheritdoc cref="EqualsAny(string, StringComparer, string[])"/>
        public static bool EqualsAny(this string target, IEnumerable<string> values)
        {
            return values.Contains(target, StringComparer.OrdinalIgnoreCase);
        }
    }


    public static class ListExtension
    {

        /// <summary>
        /// <para>
        /// An extension method that should give you exception-safe async iteration by changing the return type of the lambda from void to Task, so that exceptions will propagate up correctly
        /// </para>
        /// 
        /// <code>
        /// await db.Groups.ToList().ForEachAsync(async i => {
        ///await GetAdminsFromGroup(i.Gid);
        ///});
        /// </code>
        /// </summary>
        /// <remarks>
        ///     Source: <see href="https://stackoverflow.com/a/28996883">Stack Overflow: How can I use Async with ForEach?</see>
        /// </remarks>
        /// <returns></returns>
        public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
        {
            foreach (var value in list)
            {
                await func(value);
            }
        }
    }



}
