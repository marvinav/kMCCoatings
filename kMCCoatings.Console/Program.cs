using System;
using kMCCoatings.Core;
namespace kMCCoatings.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            var asVa = new Calculator(new kMCCoatings.Core.Configuration.CalculatorSettings());
            if(args != null)
            {                
                System.Console.WriteLine(args[0]);
            }
        }
    }
}
