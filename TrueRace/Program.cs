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
            Console.WriteLine(Data.currentRace.track.Name);
            Visualize.DrawTrack(Data.currentRace.track);
            for ( ; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}
