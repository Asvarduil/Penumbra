using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Services;
using Asvarduil.Penumbra.DataCore.Models;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands.AdminCommands
{
    public class ReadFeedbackChatCommand : BaseAdminChatCommand
    {
        public override string Token => "!READFEEDBACK";
        public override string HelpText => "Usage: !READFEEDBACK [Empty | HELP | Player].  If no args, reads all feedback.  If player name, shows that player's feedback.  If HELP, show this message.";

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
                // !READFEEDBACK [Empty | Player | HELP]
                string parsedCommand = chatCommand.Command.ToUpper();

                // Step 1 - Move past the command invocation...
                cmdArgs.Add(chatCommand.SourcePlayerName);
                parsedCommand = parsedCommand.Replace($"{Token}", string.Empty);

                // Step 2 - Trim what's left, look at what the string starts with.
                parsedCommand = parsedCommand.Trim();
                string[] remainingParts = parsedCommand.Split(' ');
                string firstPart = remainingParts[0];

                if(string.IsNullOrEmpty(firstPart))
                {
                    // Empty usage; this is valid.  Call it here.
                }
                else if(firstPart.ToUpper().StartsWith("HELP"))
                {
                    cmdArgs.Add("HELP");
                }
                else
                {
                    cmdArgs.Add(firstPart);
                }

                args = cmdArgs.ToArray();
            }

            return isThisCommand;
        }

        public override List<string> ExecuteCommand(params string[] args)
        {
            var commandFeedback = new List<string>();
            string feedbackSource = args[0];

            // If 1 argument, return all feedback.
            if (args.Length == 1)
            {
                var allFeedbacks = FeedbackService.GetAll();
                SendFeedbackPMsToQuerier(feedbackSource, commandFeedback, allFeedbacks);

                return commandFeedback;
            }

            // If 2 arguments, and the second arg is HELP, show help text.
            // Also show help message if more than 2 args used.
            bool isHelpUsage = args.Length == 2 && args[1].ToUpper() == "HELP";
            if (isHelpUsage 
                || args.Length > 2)
            {
                commandFeedback.AddMessageCommand(feedbackSource, HelpText);
                return commandFeedback;
            }

            // Otherwise, try to find the player named as the 2nd argument.
            var feedbacks = FeedbackService.GetByPlayerName(args[1]);
            SendFeedbackPMsToQuerier(feedbackSource, commandFeedback, feedbacks);
            
            return commandFeedback;
        }

        private void SendFeedbackPMsToQuerier(string feedbackSource, List<string> commandFeedback, List<Feedback> feedbacks)
        {
            if(feedbacks == null)
            {
                commandFeedback.AddMessageCommand(feedbackSource, "No results.");
                return;
            }

            foreach(var feedback in feedbacks)
            {
                commandFeedback.AddMessageCommand(feedbackSource, $"Rating: {feedback.Rating} - Details: {feedback.Details}");
            }
        }
    }
}
