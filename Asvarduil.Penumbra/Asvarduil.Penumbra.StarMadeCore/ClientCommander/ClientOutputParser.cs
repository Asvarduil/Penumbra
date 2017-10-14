using System.Collections.Generic;
using Asvarduil.Penumbra.StarMadeCore.ChatCommands;
using Asvarduil.Penumbra.StarMadeCore.ServerEventCommands;

namespace Asvarduil.Penumbra.StarMadeCore
{
    public class ClientOutputParser
    {
        #region Variables / Properties

        private string _clientOutput;

        public bool IsServerOutput => _clientOutput.Contains("[SERVER]");

        #endregion Variables / Properties

        #region Constructor

        public ClientOutputParser(string output)
        {
            _clientOutput = output;
        }

        #endregion Constructor

        #region Methods

        public List<string> RunCommand()
        {
            string[] commandArgs = null;
            var command = ChatCommander.IdentifyCommand(_clientOutput, out commandArgs);
            if (command == null)
                return null;

            var reactions = command.ExecuteCommand(commandArgs);
            return reactions;
        }

        public string ActOnServerEvent()
        {
            string[] eventArgs = null;
            var serverEvent = ServerEventCommander.IdentifyEvent(_clientOutput, out eventArgs);
            if (serverEvent == null)
                return string.Empty;

            string reaction = serverEvent.OnEventRecognized(eventArgs);
            return reaction;
        }

        #endregion Methods
    }
}
