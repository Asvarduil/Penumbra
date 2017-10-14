using System;
using System.IO;
using System.Diagnostics;

namespace Asvarduil.Penumbra.UnixCore
{
    /// <summary>
    /// Provides means by which to be able to run UNIX commands.
    /// </summary>
    public static class UnixCommand
    {
        #region Constants

        public const string UNIX_BASH_COMMAND = "/bin/bash";

        #endregion Constants

        #region Methods

        /// <summary>
        /// Using a given shell command/arguments, runs all commands in the given file name, and reports
        /// the results in a UnixCommandResult data structure.
        /// </summary>
        /// <param name="shellCommand">Command to start the shell</param>
        /// <param name="shellArguments">Arguments for running the shell</param>
        /// <param name="filePath">File with commands to run.</param>
        /// <returns>UnixCommmandResult, detailing the outcomes of the file's operations.</returns>
        public static UnixCommandResult RunFile(string shellCommand, string shellArguments, string filePath)
        {
            UnixCommandResult result = new UnixCommandResult();

            // Ready the shell process we want to run...
            var processInfo = new ProcessStartInfo(shellCommand);
            processInfo.RedirectStandardInput = true;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.WindowStyle = ProcessWindowStyle.Normal;
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;
            processInfo.Arguments = shellArguments;

            Process runningProcess = null;

            try
            {
                runningProcess = Process.Start(processInfo);

                // Read file...
                string fileContents = GetFileShellCommands(filePath);
                RunShellCommands(fileContents, runningProcess, result);
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                result.Errors += Environment.NewLine + message;
            }
            finally
            {
                if (runningProcess != null)
                    runningProcess.Close();
            }

            return result;
        }

        /// <summary>
        /// Performs basic file I/O to get a series of shell commands
        /// from a given file.
        /// </summary>
        /// <param name="filePath">File to read commands from.</param>
        /// <returns>String, containing file contents.</returns>
        private static string GetFileShellCommands(string filePath)
        {
            string fileContents = string.Empty;
            var file = new FileInfo(filePath);
            using (StreamReader reader = file.OpenText())
            {
                try
                {
                    fileContents = reader.ReadToEnd();
                }
                // For now, disregard errors...
                finally
                {
                    reader.Close();
                }
            }

            return fileContents;
        }

        /// <summary>
        /// Run a series of shell commands in the given shell process, and put the outputs and errors
        /// into the given UnixCommandResult.
        /// </summary>
        /// <param name="commands">Series of UNIX commands to run</param>
        /// <param name="shellProcess">Process handle of the shell the commands are running in</param>
        /// <param name="result">UnixCommandResult detailing the outcomes of the operation.</param>
        private static void RunShellCommands(string commands, Process shellProcess, UnixCommandResult result)
        {
            // Now, actually do the command!
            using (StreamWriter cmdWriter = shellProcess.StandardInput)
            using (StreamReader outputReader = shellProcess.StandardOutput)
            using (StreamReader errorReader = shellProcess.StandardError)
            {
                cmdWriter.AutoFlush = true;

                // Issue Command...
                try
                {
                    cmdWriter.Write(commands);
                }
                finally
                {
                    cmdWriter.Close();
                }

                // Read results...
                try
                {
                    result.Output = outputReader.ReadToEnd();
                }
                finally
                {
                    outputReader.Close();
                }

                // Read errors...
                try
                {
                    result.Errors = errorReader.ReadToEnd();
                }
                finally
                {
                    errorReader.Close();
                }
            }
        }

        #endregion Methods
    }
}
