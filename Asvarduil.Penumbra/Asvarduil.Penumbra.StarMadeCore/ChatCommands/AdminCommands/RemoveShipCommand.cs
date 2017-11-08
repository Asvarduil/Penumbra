using System;
using System.Collections.Generic;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands.AdminCommands
{
    public class RemoveShipCommand : BaseAdminChatCommand
    {
        public override string Token => "!REMOVESHIP";
        public override string HelpText => "Usage: !REMOVESHIP [ShipName|HELP].  This command deletes all ships that start with the given ship name.";

        public override bool TryParseCommand(string rawData, out string[] args)
        {
            args = null;
            var cmdArgs = new List<string>();

            DestructuredChatCommand chatCommand = null;
            bool isChatCommand = ChatCommander.TryParseCommand(rawData, out chatCommand);
            if (!isChatCommand)
                return false;

            bool isThisCommand = chatCommand.Command.StartsWith(Token);
            if (isThisCommand)
            {
                // !REMOVESHIP [ShipName]
                // Step 1 - Remove token...
                cmdArgs.Add(chatCommand.SourcePlayerName);
                var parsedCommand = chatCommand.Command.Replace($"{Token} ", string.Empty);

                if (string.IsNullOrWhiteSpace(parsedCommand)
                    || parsedCommand.Trim().ToLower() == "help")
                    cmdArgs.Add("HELP");
                else
                {
                    string shipName = parsedCommand.Trim();
                    cmdArgs.Add(shipName);
                }

                args = cmdArgs.ToArray();
            }

            return isThisCommand;
        }

        public override List<string> ExecuteCommand(params string[] args)
        {
            var commandFeedback = new List<string>();

            string feedbackSource = args[0];
            if (args.Length != 2
                || args[1].ToUpper() == "HELP")
            {
                commandFeedback.AddMessageCommand(feedbackSource, HelpText);
                return commandFeedback;
            }

            // Run /despawn_all against all ships that start with the ship name, used or unused.
            string shipName = args[1];
            commandFeedback.Add($"/despawn_all \"{shipName}\" all true");
            commandFeedback.AddMessageCommand(feedbackSource, $"Removed all ships that start with {shipName}.");

            return commandFeedback;
        }
    }
}
