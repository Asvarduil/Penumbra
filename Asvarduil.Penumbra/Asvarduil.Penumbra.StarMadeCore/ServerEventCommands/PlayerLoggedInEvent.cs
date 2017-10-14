using System;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.StarMadeCore.ServerEventCommands
{
    public class PlayerLoggedInEvent : IServerEvent
    {
        // Example:
        // [LOGIN] successful auth: now protecting username Asvarduil with null

        public bool TryParseServerEvent(string rawData, out string[] eventArgs)
        {
            eventArgs = new string[1];
            if (!rawData.StartsWith("[LOGIN] successful auth: now protecting username"))
                return false;

            string parsedEvent = rawData.Replace("[LOGIN] successful auth: now protecting username ", string.Empty);
            parsedEvent = parsedEvent.Replace(" with null", string.Empty);

            string loggedInPlayer = parsedEvent;

            eventArgs[0] = loggedInPlayer;
            return true;
        }

        public string OnEventRecognized(string[] eventArgs)
        {
            string reaction = string.Empty;

            string playerName = eventArgs[0];
            var existingPlayer = PlayerService.GetByName(playerName);
            if(existingPlayer != null)
            {
                existingPlayer.LastLoggedInDate = DateTime.Now;
                PlayerService.Update(existingPlayer);

                reaction = $"/server_message_broadcast info \"Welcome back, {existingPlayer.Name}!\"";
                return reaction;
            }

            // Player doesn't exist, create them.
            PlayerService.Create(playerName);

            reaction = $"/server_message_broadcast info \"Welcome {playerName} to MicroMade!\"";
            return reaction;
        }
    }
}
