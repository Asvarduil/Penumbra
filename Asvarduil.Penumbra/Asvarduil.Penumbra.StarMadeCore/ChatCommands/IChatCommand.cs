using System.Collections.Generic;

namespace Asvarduil.Penumbra.StarMadeCore.ChatCommands
{
    /// <summary>
    /// Describes the form of all Chat Commands in the MicroMade mod.
    /// </summary>
    public interface IChatCommand
    {
        /// <summary>
        /// Does this command require admin rights to run?
        /// </summary>
        bool IsAdminCommand { get; }

        /// <summary>
        /// Token that denotes this command.
        /// </summary>
        string Token { get; }

        /// <summary>
        /// Help text to show on invalid arg count, or the HELP keyword used as an argument.
        /// </summary>
        string HelpText { get; }

        /// <summary>
        /// Checks the using player to see if they're allowed to use this command.
        /// </summary>
        /// <param name="usingPlayerName">Player invoking the command</param>
        /// <returns>True if authorized, otherwise false.</returns>
        bool IsAuthorized(string usingPlayerName);

        /// <summary>
        /// Sifts the raw data for characteristics that identify the specific command.
        /// </summary>
        /// <param name="rawData">String data from the process' standard output</param>
        /// <param name="args">Emitted argument list parsed from the raw data</param>
        /// <returns>True if the command is identified, otherwise false.</returns>
        bool TryParseCommand(string rawData, out string[] args);

        /// <summary>
        /// Runs the given command, with the given set of arguments.
        /// </summary>
        /// <param name="args">String arguments used to invoke the command</param>
        /// <returns>Command to run as a consequence of this command.</returns>
        List<string> ExecuteCommand(params string[] args);
    }
}
