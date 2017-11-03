using System;
using System.Collections.Generic;
using Asvarduil.Penumbra.StarMadeCore.ChatCommands.AdminCommands;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands
{
    public static class ChatCommander
    {
        #region Variables / Properties

        private static List<IChatCommand> CommandRegistry = new List<IChatCommand>
        {
            new FeedbackChatCommand(),
            new NetWorthChatCommand(),
            new CashoutChatCommand(),
            new BountyChatCommand()
        };

        private static List<BaseAdminChatCommand> AdminCommandRegistry = new List<BaseAdminChatCommand>
        {
            new ShutdownChatCommand(),
            new RestartChatCommand(),
            new ReadFeedbackChatCommand(),
            new AdminPromoteChatCommand(),
            new AdminDemoteChatCommand(),
            new RemoveBountyChatCommand(),
            new GrantNetWorthChatCommand(),
            new RemoveShipCommand()
        };

        #endregion Variables / Properties

        #region Methods

        public static bool TryParseCommand(string rawData, out DestructuredChatCommand command)
        {
            command = null;
            if (!IsMessage(rawData))
                return false;

            command = ParseCommand(rawData);
            return command.IsCommand;
        }

        private static bool IsMessage(string rawData)
        {
            return rawData.StartsWith("[CHANNELROUTER] RECEIVED MESSAGE ON");
        }

        private static DestructuredChatCommand ParseCommand(string rawData)
        {
            // Split on ][, in order to isolate parts of the command.
            // [0]: [CHANNELROUTER] RECEIVED MESSAGE ON Server(0): [CHAT
            // [1]: sender=Retrom_Gall
            // [2]: receiverType=CHANNEL
            // [3]: receiver=Server
            // [4]: message=!FEEDBACK 1 Test]
            string[] dataParts = rawData.Split(new[] { "][" }, StringSplitOptions.None);
            string sourcePlayerName = dataParts[1].Replace("sender=", string.Empty);

            // Get command...
            string command = dataParts[4].Replace("message=", string.Empty);
            // Trim off final ] of command...
            command = command.Substring(0, command.Length - 1);

            var result = new DestructuredChatCommand
            {
                SourcePlayerName = sourcePlayerName,
                Command = command
            };

            return result;
        }

        public static IChatCommand IdentifyCommand(string rawData, out string[] args)
        {
            IChatCommand result = null;
            args = null;

            // Identify basic commands.
            foreach (var command in CommandRegistry)
            {
                bool isCommand = command.TryParseCommand(rawData, out args);
                if (!isCommand)
                    continue;

                result = command;
                break;
            }

            if(result != null)
                return result;

            // Identify admin commands.
            foreach(var command in AdminCommandRegistry)
            {
                bool isCommand = command.TryParseCommand(rawData, out args);
                if (!isCommand)
                    continue;

                if (!command.IsAuthorized(args[0]))
                    continue;

                result = command;
                break;
            }

            return result;
        }

        #endregion Methods
    }
}
