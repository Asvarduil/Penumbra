using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands.AdminCommands
{
    public class GrantNetWorthChatCommand : BaseAdminChatCommand
    {
        public override string Token => "!GRANTNETWORTH";
        public override string HelpText => "Usage: !GRANTNETWORTH [Player] [Value].  Administrator grants Player an additional value of Net Worth.";

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
                if (parts.Length != 2)
                    cmdArgs.Add("HELP");
                else
                {
                    string targetPlayerName = parts[0];
                    cmdArgs.Add(targetPlayerName);

                    string value = parts[1];
                    cmdArgs.Add(value);
                }

                args = cmdArgs.ToArray();
            }

            return isThisCommand;
        }

        public override List<string> ExecuteCommand(params string[] args)
        {
            var commandFeedback = new List<string>();

            string feedbackSource = args[0];
            if (args.Length != 3
                || args[1].ToUpper() == "HELP")
            {
                commandFeedback.AddMessageCommand(feedbackSource, HelpText);
                return commandFeedback;
            }

            string targetPlayer = args[1];
            var player = PlayerService.GetByName(targetPlayer);
            if(player == null)
            {
                commandFeedback.AddMessageCommand(feedbackSource, $"{targetPlayer} does not exist.");
                return commandFeedback;
            }
            
            int value;
            bool isValid = int.TryParse(args[2], out value);
            if(!isValid)
            {
                commandFeedback.AddMessageCommand(feedbackSource, $"{args[2]} isn't a number.");
                return commandFeedback;
            }

            string messageVerb = value > 0
                ? "bestowed upon"
                : "deducted from";

            PlayerService.IncreaseNetWorth(targetPlayer, value);

            commandFeedback.AddMessageCommand(targetPlayer, $"You have had a personal value of {value} Credits {messageVerb} you.  Use !NETWORTH ME to check your new net worth.");
            return commandFeedback;
        }
    }
}
