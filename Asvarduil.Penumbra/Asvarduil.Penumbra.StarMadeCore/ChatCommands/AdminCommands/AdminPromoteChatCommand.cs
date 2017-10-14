using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands.AdminCommands
{
    public class AdminPromoteChatCommand : BaseAdminChatCommand
    {
        public override string Token => "!PROMOTEADMIN";
        public override string HelpText => "Usage: !PROMOTEADMIN [Player].  Makes the player a server admin, which lets them use potent admin ! commands.";

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
                // !REMOVEBOUNTY [PlayerName]
                // Step 1 - Move past the command invocation...
                cmdArgs.Add(chatCommand.SourcePlayerName);
                var parsedCommand = chatCommand.Command.Replace($"{Token} ", string.Empty);

                var parts = parsedCommand.Split(' ');
                if (parts.Length > 1)
                    cmdArgs.Add("HELP");
                else
                {
                    string targetPlayerName = parts[0];
                    cmdArgs.Add(targetPlayerName);
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

            // Remove the bounty from the named player.
            string promotedPlayer = args[1];
            PlayerService.PromoteToAdmin(promotedPlayer);

            commandFeedback.AddBroadcastCommand($"{promotedPlayer} is now a MicroMade Administrator!");
            return commandFeedback;
        }
    }
}
