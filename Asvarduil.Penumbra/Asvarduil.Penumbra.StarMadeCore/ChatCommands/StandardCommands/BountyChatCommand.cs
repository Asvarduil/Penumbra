using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands
{
    public class BountyChatCommand : IChatCommand
    {
        public bool IsAdminCommand => false;
        public string Token => "!BOUNTY";
        public string HelpText => "Usage: !BOUNTY [TargetName] [Amount].  Puts a bounty with amount on TargetName.  Amount can't exceed your Net Worth.  Use !NETWORTH ME to see your net worth.";

        public bool IsAuthorized(string usingPlayerName)
        {
            return true;
        }

        public bool TryParseCommand(string rawData, out string[] args)
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
                // !BOUNTY [Player] [Amount]
                // Step 1 - Move past the command invocation...
                cmdArgs.Add(chatCommand.SourcePlayerName);
                var parsedCommand = chatCommand.Command.Replace($"{Token} ", string.Empty);

                // Step 2 - Separate the Player from the Amount...
                // If there aren't two arguments, default to HELP mode.
                string[] cmdParts = parsedCommand.Split(' ');
                if(cmdParts.Length != 2)
                {
                    cmdArgs.Add("HELP");
                }
                else
                {
                    string targetPlayerName = cmdParts[0];
                    string amount = cmdParts[1];

                    cmdArgs.Add(targetPlayerName);
                    cmdArgs.Add(amount);
                }

                args = cmdArgs.ToArray();
            }

            return isThisCommand;
        }

        public List<string> ExecuteCommand(params string[] args)
        {
            var commandFeedback = new List<string>();

            string feedbackSource = args[0];
            if(args.Length == 2)
            {
                commandFeedback.AddMessageCommand(feedbackSource, HelpText);
                return commandFeedback;
            }

            string targetPlayerName = args[1];

            // Step 1 - Is the bounty amount a number?
            int amount;
            bool isNumber = int.TryParse(args[2], out amount);
            if(!isNumber)
            {
                commandFeedback.AddMessageCommand(feedbackSource, $"{args[2]} is not a number.");
                return commandFeedback;
            }

            // Step 2 - Bounty is generated for the target...
            var result = PlayerService.PostBounty(feedbackSource, targetPlayerName, amount);
            if(!result.IsSuccessful)
            {
                commandFeedback.AddMessageCommand(feedbackSource, result.Message);
                return commandFeedback;
            }

            commandFeedback.AddBroadcastCommand($"A bounty has been posted for {targetPlayerName}!");
            return commandFeedback;
        }
    }
}
