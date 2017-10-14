using System;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.Daemons
{
    /// <summary>
    /// This daemon is responsible for periodically increasing the net worth of all logged
    /// on players.
    /// </summary>
    public class PeriodicNetWorthDaemon : IDaemon
    {
        public string Name => "Periodic NetWorth Daemon";
        public DateTime LastRan { get; set; }
        public int Period => 60; // TODO: Draw from configuration file or database.

        public void OnInvoked()
        {
            var onlinePlayers = PlayerService.GetLoggedOnPlayers();
            foreach(var player in onlinePlayers)
            {
                PlayerService.IncreaseNetWorth(player.Name, 100);
            }
        }
    }
}
