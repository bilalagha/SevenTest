using Microsoft.Extensions.Configuration;
using SevenTest.ConsoleOutput.Helper;
using System;

namespace SevenTest.ConsoleOutput
{
    class Program
    {
        const string BaseURLConfigKey = "PersonAPIUrl";
        static void Main(string[] args)
        {

        IConfiguration Configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables()
           .AddCommandLine(args)
           .Build();
            

            Console.WriteLine("Seven West Media Technical Assingment Console Output");
            Console.WriteLine("====================================================");


            var personConsoleHelper = new PersonConsoleHelper(Configuration[BaseURLConfigKey].ToString());

            personConsoleHelper.Write7TestOutput();
        }
    }
}
