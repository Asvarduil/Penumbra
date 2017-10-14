using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands.AdminCommands
{
    /// <summary>
    /// The basic form of every Administrator command in the MicroMade mod.
    /// </summary>
    public abstract class BaseAdminChatCommand : IChatCommand
    {
        public virtual bool IsAdminCommand => true;

        public abstract string Token { get; }
        public abstract string HelpText { get; }

        public virtual bool IsAuthorized(string usingPlayerName)
        {
            return PlayerService.IsAdmin(usingPlayerName);
        }

        public abstract bool TryParseCommand(string rawData, out string[] args);
        public abstract List<string> ExecuteCommand(params string[] args);
    }
}
