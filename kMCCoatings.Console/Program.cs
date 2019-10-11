using System;
using kMCCoatings.Core;
namespace kMCCoatings.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            if(args != null)
            {                
                System.Console.WriteLine(args[0]);
            }
        }
    }
}
