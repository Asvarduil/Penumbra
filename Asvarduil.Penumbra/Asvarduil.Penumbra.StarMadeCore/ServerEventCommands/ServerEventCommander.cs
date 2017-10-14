using System.Collections.Generic;

namespace Asvarduil.Penumbra.StarMadeCore.ServerEventCommands
{
    public static class ServerEventCommander
    {
        // Programmer's Notes:
        // -------------------
        // Examples:
        // Login:                     [LOGIN] successful auth: now protecting username Asvarduil with null
        // Logout:                    [SERVER] onLoggedOut DONE for RegisteredClient: Asvarduil (1) connected: true
        // Ship Core Entered:         [CONTROLLER][ADD-UNIT] (Server(0)): PlS[Asvarduil ; id(2)(1)f(0)] Added to controllers: Ship[Coalition Legionnaire Scipio](34)
        // Ship Core Exited:          [CONTROLLER][ADD-UNIT] (Server(0)): PlS[Asvarduil ; id(2)(1)f(0)] Added to controllers: PlayerCharacter[(ENTITY_PLAYERCHARACTER_Asvarduil)(192)]
        // Death occurred:            [SERVER] PLAYER PlS[Asvarduil ; id(2)(1)f(0)] died, removing ALL control units
        // Death to other player:     
        // Death to environment:      
        // Death to ship by player:   
        // Opened Inventory:          
        // Trigger Entered:           
        // Server Shutdown Completed: 

        public static List<IServerEvent> RegisteredEvents = new List<IServerEvent>
        {
            new PlayerLoggedInEvent(),
            new PlayerLoggedOutEvent()
        };

        public static bool IsIgnoredEvent(string rawData)
        {
            // TODO: To improve performance, look for certain tags that we'd like to ignore...
            return false;
        }

        public static IServerEvent IdentifyEvent(string rawData, out string[] eventArgs)
        {
            eventArgs = null;
            IServerEvent result = null;

            foreach(var registeredEvent in RegisteredEvents)
            {
                if (!registeredEvent.TryParseServerEvent(rawData, out eventArgs))
                    continue;

                result = registeredEvent;
                break;
            }

            return result;
        }
    }
}
