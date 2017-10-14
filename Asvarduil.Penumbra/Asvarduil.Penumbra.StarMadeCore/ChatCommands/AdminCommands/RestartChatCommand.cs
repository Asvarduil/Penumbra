using System.Collections.Generic;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands.AdminCommands
{
    public class RestartChatCommand : BaseAdminChatCommand
    {
        public override string Token => "!RESTART";
        public override string HelpText => "Usage: !RESTART [Duration].  Shuts the server down after starting a countdown for all players on the server.  Restarts after shutdown.";

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
            int duration;
            bool isValid = int.TryParse(args[1], out duration);
            if (!isValid)
            {
                commandFeedback.AddMessageCommand(feedbackSource, $"{args[1]} is not a number.");
                return commandFeedback;
            }

            ClientCommander.Instance.Restart(duration);

            commandFeedback.AddBroadcastCommand("MicroMade is being restarted.  Please log out soon.");
            return commandFeedback;
        }
    }
}
