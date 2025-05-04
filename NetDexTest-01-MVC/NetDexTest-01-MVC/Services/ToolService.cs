namespace NetDexTest_01_MVC.Services
{
    public class ToolService : IToolService
    {
        private readonly ILogger<ToolService> _logger;

        public ToolService(ILogger<ToolService> logger)
        {
            _logger = logger;
        }



        public string Writer(string input)
        {
            string output = "";
            output =
                  "\n\n"
                + "--------------------"
                + "\n\n"
                + $"{input}"
                + "\n\n\n"
                + "-------------------"
                + "\n\n";
            _logger.LogInformation(output);
            return output;
        }
        
        public string Writer(Exception ex)
        {
            string output = "";
            output =
                  "\n\n"
                + "------EXCEPTION-----"
                + "\n\n"
                + $"\t{ex.Message}"
                + "\n\n"
                + "--------------------"
                + "\n\n"
                + $"TARGET SITE:\n\t{ex.TargetSite}"
                + "\n\n\n"
                + $"SOURCE:\n\t{ex.Source}"
                + "\n\n\n"
                + "--end---EXCEPTION--"
                + "\n\n";
            _logger.LogError(output);
            return output;
        }

        public async Task ConOut(string input)
        {
            string stringhere = Writer(input);
            _logger.LogInformation(stringhere);
            
            await Console.Out.WriteLineAsync(stringhere);
        }

        public async Task ConOut(string input, Exception ex)
        {   
            await Console.Out.WriteLineAsync($"{Writer(input)}");
            await Console.Out.WriteLineAsync($"{Writer(ex)}");
        }

    }
}
