using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newegg.WMS.JobConsole.EmailNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "WMS Job Console";
            Console.WriteLine("Processing...");
            Bootstrapper.Error += new Bootstrapper.EventHandler<string>(Instance_Error);
            Bootstrapper.Completed += new Bootstrapper.EventHandler<string>(Instance_Completed);
            Bootstrapper.Start();
            //Console.ReadKey();
        }

        private static void Instance_Completed(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
        }

        private static void Instance_Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
        }
    }
}
