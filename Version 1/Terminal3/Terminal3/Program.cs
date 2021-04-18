using System;

namespace Terminal3
{
    class Program
    {
        static void Main(string[] args)
        {
            DomainLayer.Logger.LogInfo("My Log Info");
            DomainLayer.Logger.LogError("My Error Info");
            Console.WriteLine("Hello World!");
            Console.ReadKey();
            
        }
    }
}
