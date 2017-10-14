using System;
using System.Threading;
using System.Collections.Generic;

namespace Asvarduil.Penumbra.Daemons
{
    /// <summary>
    /// Manages all Daemons that run in Penumbra.
    /// </summary>
    public class DaemonLord
    {
        #region Variables / Properties

        public static bool RunDaemons = true;

        public static List<IDaemon> RegisteredDaemons = new List<IDaemon>
        {
            new PeriodicNetWorthDaemon()
        };

        public static Thread Thread;

        #endregion Variables / Properties

        #region Methods

        public static Thread CreateThread()
        {
            var daemonThreadStarter = new ThreadStart(InvokeAllDaemons);
            Thread = new Thread(daemonThreadStarter);
            Thread.Start();

            return Thread;
        }

        public static void InvokeAllDaemons()
        {
            do
            {
                // Sleep for 10s between checks...
                Thread.Sleep(10000);

                DateTime now = DateTime.Now;
                foreach(var daemon in RegisteredDaemons)
                {
                    if (now < daemon.LastRan.AddSeconds(daemon.Period))
                        continue;

                    Console.WriteLine($"[{now.ToShortTimeString()}]: {daemon.Name} has been triggered!");

                    daemon.LastRan = now;
                    try
                    {
                        daemon.OnInvoked();
                    }
                    catch(Exception ex)
                    {
                        // Daemons should never halt the server.  Instead, we should report the failure, and attempt to
                        // keep chugging along.

                        // TODO: Better error handling...
                        Console.WriteLine($"Exception: {ex.Message}\r\n{ex.StackTrace}");
                    }
                }
            } while (RunDaemons);
        }

        #endregion Methods
    }
}
