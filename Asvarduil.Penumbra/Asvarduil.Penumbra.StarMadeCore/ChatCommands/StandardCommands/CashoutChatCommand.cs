using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands
{
    public class CashoutChatCommand : IChatCommand
    {
        public bool IsAdminCommand => false;
        public string Token => "!CASHOUT";
        public string HelpText => "Usage: !CASHOUT.  Immediately clears your net worth, and causes you to earn that many Credits.";

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
                // !CASHOUT
                string parsedCommand = chatCommand.Command.ToUpper();

                // Step 1 - Move past the command invocation...
                cmdArgs.Add(chatCommand.SourcePlayerName);
                parsedCommand = parsedCommand.Replace($"{Token}", string.Empty);

                // Step 2 - If there's anything left over, show Help text.
                parsedCommand = parsedCommand.Trim();
                if (!string.IsNullOrEmpty(parsedCommand))
                    cmdArgs.Add("HELP");

                args = cmdArgs.ToArray();
            }

            return isThisCommand;
        }

        public List<string> ExecuteCommand(params string[] args)
        {
            var commandFeedback = new List<string>();

            string feedbackSource = args[0];
            if (args.Length == 2 && args[1] == "HELP")
            {
                commandFeedback.AddMessageCommand(feedbackSource, HelpText);
                return commandFeedback;
            }

            var player = PlayerService.GetByName(feedbackSource);
            if(player == null
               || player.NetWorth == null)
            {
                commandFeedback.AddMessageCommand(feedbackSource, "You are missing MicroMade server records.  Contact an administrator ASAP.");
                return commandFeedback;
            }

            int value = player.NetWorth.Value;

            // Clear out Net Worth...
            player.NetWorth.Value = 0;
            PlayerService.Update(player);

            commandFeedback.Add($"/give_credits {feedbackSource} {value}");
            return commandFeedback;
        }
    }
}
