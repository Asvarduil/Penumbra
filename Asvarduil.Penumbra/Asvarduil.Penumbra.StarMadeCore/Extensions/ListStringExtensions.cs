using System.Collections.Generic;

namespace Asvarduil.Penumbra.StarMadeCore
{
    public static class ListStringExtensions
    {
        /// <summary>
        /// Adds a /pm [recipient] "[message]" command to a list of string commands.
        /// </summary>
        /// <param name="list">List to add the command to.</param>
        /// <param name="recipientPlayerName">Name of the player to send a message to</param>
        /// <param name="message">Message to send</param>
        public static void AddMessageCommand(this List<string> list, string recipientPlayerName, string message, string severity = "info")
        {
            list.Add($"/server_message_to {severity} {recipientPlayerName} \"{message}\"");
        }

        /// <summary>
        /// Adds a /server_message_broadcast info "[message]" command to a list of string commands.
        /// </summary>
        /// <param name="list">List to add the command to.</param>
        /// <param name="message">Message that will be broadcast as info to all server players.</param>
        public static void AddBroadcastCommand(this List<string> list, string message, string severity = "info")
        {
            list.Add($"/server_message_broadcast {severity} \"{message}\"");
        }
    }
}
