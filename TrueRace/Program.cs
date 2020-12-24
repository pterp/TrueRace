using System;
using System.Threading;
using Controller;
namespace TrueRace
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Data.initialize();
            Data.NextRace();
            Visualize.Initialize();
            Data.currentRace.StartVisualize();

            for ( ; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}
