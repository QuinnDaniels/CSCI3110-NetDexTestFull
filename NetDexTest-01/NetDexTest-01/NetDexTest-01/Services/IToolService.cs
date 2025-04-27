namespace NetDexTest_01.Services
{
    public interface IToolService
    {

        string Writer(string input);

        Task ConOut(string input);

        string Writer(Exception ex);

        Task ConOut(string input, Exception ex);

    }
}
