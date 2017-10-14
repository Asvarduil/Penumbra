using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands.AdminCommands
{
    public class RemoveBountyChatCommand : BaseAdminChatCommand
    {
        public override string Token => "!REMOVEBOUNTY";
        public override string HelpText => "Usage: !REMOVEBOUNTY [Player].  Remove all bounties from the named player.";

        public override bool TryParseCommand(string rawData, out string[] args)
        {
            args = null;
            var cmdArgs = new List<string>();

            DestructuredChatCommand chatCommand = null;
            bool isChatCommand = ChatCommander.TryParseCommand(rawData, out chatCommand);
            if (!isChatCommand)
                return false;

            bool isThisCommand = chatCommand.Command.StartsWith(Token);
            if(isThisCommand)
            {
                // !REMOVEBOUNTY [PlayerName]
                // Step 1 - Move past the command invocation...
                cmdArgs.Add(chatCommand.SourcePlayerName);
                var parsedCommand = chatCommand.Command.Replace($"{Token} ", string.Empty);

                // Step 2 - Use what's left as the argument.
                //          If nothing's left, use the HELP keyword instead.
                if (string.IsNullOrEmpty(parsedCommand))
                    cmdArgs.Add("HELP");
                else
                    cmdArgs.Add(parsedCommand);

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
            string pardonedPlayer = args[1];
            var result = PlayerService.RemoveBounty(pardonedPlayer);
            if(!result.IsSuccessful)
            {
                commandFeedback.AddMessageCommand(feedbackSource, result.Message);
                return commandFeedback;
            }

            commandFeedback.AddBroadcastCommand($"It has been decreed that {pardonedPlayer} no longer has a bounty on them.");
            return commandFeedback;
        }
    }
}
