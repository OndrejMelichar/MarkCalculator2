using System;

namespace ConsoleDebugApp
{
    class Program
    {
        static void Main(string[] args)
        {
            App app = new App();
            app.Start();
            Console.WriteLine(">-- KONEC -----------------------------");
            Console.ReadKey();
        }
    }
}
