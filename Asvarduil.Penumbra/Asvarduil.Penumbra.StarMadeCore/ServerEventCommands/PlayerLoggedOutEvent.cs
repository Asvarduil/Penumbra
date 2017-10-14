using System;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.StarMadeCore.ServerEventCommands
{
    public class PlayerLoggedOutEvent : IServerEvent
    {
        // Example:
        // [SERVER] onLoggedOut DONE for RegisteredClient: Asvarduil (1) connected: true

        public bool TryParseServerEvent(string rawData, out string[] eventArgs)
        {
            eventArgs = new string[1];
            if(!rawData.StartsWith("[SERVER] onLoggedOut DONE"))
                return false;

            string parsedData = rawData.Replace("[SERVER] onLoggedOut DONE for Registered Client: ", "");
            parsedData = parsedData.Replace(" connected: true", "");
            // Remaining: [UserName] ([#])

            string[] parts = parsedData.Split(' ');
            eventArgs[0] = parts[0];
            
            return true;
        }

        public string OnEventRecognized(string[] eventArgs)
        {
            string playerName = eventArgs[0];
            var player = PlayerService.GetByName(playerName);
            if (player == null)
            {
                // TODO: Notify of error.
                return string.Empty;
            }

            player.LastLoggedOutDate = DateTime.Now;
            PlayerService.Update(player);

            return $"/server_message_broadcast info \"{playerName} has logged off.\"";
        }
    }
}
