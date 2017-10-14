using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands
{
    public class NetWorthChatCommand : IChatCommand
    {
        public bool IsAdminCommand => false;
        public string Token => "!NETWORTH";
        public string HelpText => "Usage: !NETWORTH [ME | HELP].  !NETWORTH ME shows you your net worth."
                                + " !NETWORTH HELP shows you this message.  Net Worth is gained by defeating"
                                + " other players, or by simply playing on the server.";

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
                // !NETWORTH ME|HELP
                string parsedCommand = chatCommand.Command.ToUpper();

                // Step 1 - Move past the command invocation...
                cmdArgs.Add(chatCommand.SourcePlayerName);
                parsedCommand = parsedCommand.Replace($"{Token} ", string.Empty);

                // Step 2 - Trim what's left, look at what the string starts with.
                parsedCommand = parsedCommand.Trim();
                if (parsedCommand.StartsWith("ME"))
                {
                    cmdArgs.Add("ME");
                }
                else if (parsedCommand.StartsWith("HELP"))
                {
                    cmdArgs.Add("HELP");
                }
                else
                {
                    cmdArgs.Add("HELP");
                }

                args = cmdArgs.ToArray();
            }

            return isThisCommand;
        }

        public List<string> ExecuteCommand(params string[] args)
        {
            var commandFeedback = new List<string>();

            string feedbackSource = args[0];
            if(args.Length != 2
               || args[1] == "HELP")
            {
                commandFeedback.AddMessageCommand(feedbackSource, HelpText);
                return commandFeedback;
            }

            var player = PlayerService.GetByName(feedbackSource);
            if (player == null
                || player.NetWorth == null)
            {
                commandFeedback.AddMessageCommand(feedbackSource, "You don't have a player record in the MicroMade mod.  Contact the administrators via PM ASAP.");
                return commandFeedback;
            }

            commandFeedback.AddMessageCommand(feedbackSource, $"Your net worth is {player.NetWorth.Value}.  You can withdraw this by using !CASHOUT.");
            return commandFeedback;
        }
    }
}
