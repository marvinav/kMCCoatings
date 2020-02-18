using System;
using kMCCoatings.Core;
namespace kMCCoatings.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            if (args != null)
            {
                System.Console.WriteLine(args[0]);
            }
        }
    }
}
