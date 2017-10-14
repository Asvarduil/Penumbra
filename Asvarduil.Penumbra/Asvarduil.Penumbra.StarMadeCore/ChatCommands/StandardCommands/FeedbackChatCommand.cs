using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands
{
    public class FeedbackChatCommand : IChatCommand
    {
        public bool IsAdminCommand => false;
        public string Token => "!FEEDBACK";
        public string HelpText => "Usage: !FEEDBACK [HELP][Rating 1-5] [Details].  !FEEDBACK HELP shows this message."
                                + "Rating must be between 1 and 5.  Details are anything you want to inform us of.";

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
            if (isThisCommand)
            {
                // !FEEDBACK [HELP | 1-5] [Details...]
                string parsedCommand = chatCommand.Command.ToUpper();

                // Step 1 - Move past the command invocation...
                cmdArgs.Add(chatCommand.SourcePlayerName);
                parsedCommand = parsedCommand.Substring(Token.Length + 1, parsedCommand.Length - (Token.Length + 1));

                // Step 2 - Get the rating...
                var firstArgParts = parsedCommand.Split(' ');
                if(firstArgParts[0] == "HELP")
                {
                    cmdArgs.Add(firstArgParts[0]);
                }
                else
                {
                    short rating;
                    bool isNumeric = short.TryParse(firstArgParts[0], out rating);
                    if (isNumeric && rating > 0 && rating < 6)
                        cmdArgs.Add(firstArgParts[0]);
                    else
                        cmdArgs.Add("HELP");
                }

                // Step 3 - Remove the first arg, and use the rest of the string as the details.
                int firstArgEndPosition = firstArgParts[0].Length + 1;
                parsedCommand = parsedCommand
                    .Substring(firstArgEndPosition, parsedCommand.Length - firstArgEndPosition)
                    .Trim();

                if (!string.IsNullOrEmpty(parsedCommand))
                    cmdArgs.Add(parsedCommand);

                args = cmdArgs.ToArray();
            }

            return isThisCommand;
        }

        public List<string> ExecuteCommand(params string[] args)
        {
            var commandFeedback = new List<string>();

            string feedbackSource = args[0];
            if ((args.Length < 2 || args.Length > 3)
               || args[1] == "HELP")
            {
                commandFeedback.AddMessageCommand(feedbackSource, HelpText);
                return commandFeedback;
            }

            string unparsedRating = args[1];
            string details = args[2];

            int rating;
            bool isRatingNumber = int.TryParse(unparsedRating, out rating);
            bool isValidRating = isRatingNumber && rating > 0 && rating < 6;
            if(!isValidRating)
            {
                commandFeedback.AddMessageCommand(feedbackSource, $"Your rating needs to be between 1 and 5, where 1 is really bad, and 5 is really good.");
                return commandFeedback;
            }

            FeedbackService.Create(feedbackSource, rating, details);
            commandFeedback.AddMessageCommand(feedbackSource, $"Your feedback has been received.  Thanks for telling us about what's going on!");

            return commandFeedback;
        }
    }
}
