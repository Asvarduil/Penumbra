using System;
using Asvarduil.Penumbra.Daemons;
using Asvarduil.Penumbra.StarMadeCore;

namespace Asvarduil.Penumbra
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.WriteLine("Penumbra is started.");

            using (var commander = new ClientCommander())
            {
                DaemonLord.CreateThread();

                try
                {
                    commander.OpenCommandPrompt();
                    commander.RunStarMadeServer();

                    commander.PerformStartupActions();

                    Console.WriteLine("\r\nStarMade is running.");
                    Console.WriteLine("Admins can use !RESTART [Duration] and !SHUTDOWN [Duration] from a StarMade client.");

                    // Block the process from ending until the commander
                    // is ready to 'die' off.
                    while (commander.ShouldKeepLiving)
                    {
                    }

                    DaemonLord.RunDaemons = false;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    Console.WriteLine("\r\nPress any key to exit.");
                    Console.ReadLine();
                }
                finally
                {
                    // TODO: Dispatch Error Email or create server log dump.
                    DaemonLord.Thread.Abort();
                    commander.ForceKill();
                }
            }

            Console.WriteLine("Penumbra is shut down.  Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
