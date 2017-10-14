using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Services;
using Asvarduil.Penumbra.Services;
using Asvarduil.Penumbra.Services.Extensions;
using Asvarduil.Penumbra.StarMadeCore.ChatCommands;

namespace Asvarduil.Penumbra.StarMadeCore
{
    public class ClientCommander : IDisposable
    {
        #region Variables / Properties

        public static ClientCommander Instance { get; private set; }

        private bool? _researchMode;
        public bool ResearchMode
        {
            get
            {
                if (_researchMode == null)
                {
                    string environmentSwitch = SettingsService.ReadEnvironment("Research Mode", false);
                    if (string.IsNullOrEmpty(environmentSwitch))
                    {
                        _researchMode = false;
                    }
                    else
                    {
                        _researchMode = environmentSwitch.ToUpper() == "TRUE";
                    }
                }

                return (bool) _researchMode;
            }
        }

        /// <summary>
        /// Returns the state of the StarMade shell process being used by this system.
        /// </summary>
        public bool IsClientAlive => _starMadeProcess != null;

        /// <summary>
        /// Sentinel for the main process to use to know when to stop running.
        /// </summary>
        public bool ShouldKeepLiving { get; private set; } = true;

        private Process _starMadeProcess;
        private StreamWriter _processInput;

        private string StarMadePathCmd => SettingsService.ReadEnvironment("StarMade Path Command", true);
        private string StarMadeRunCmd => SettingsService.ReadEnvironment("StarMade Run Command", true);

        private List<IChatCommand> RegisteredCommands = new List<IChatCommand>
        {

        };

        #endregion Variables / Properties

        #region Constructor

        public ClientCommander()
        {
            // Only allow one 'instance' at a time.
            if (Instance != null)
                throw new ApplicationException("Trying to construct a new Client Commander, despite the old one not being disposed!");

            Instance = this;
        }

        #endregion Constructor

        #region Methods

        public void OpenCommandPrompt()
        {
            var shellCommand = SettingsService.ReadEnvironment("StarMade Shell Command", true);

            var processInfo = new ProcessStartInfo();
            processInfo.FileName = shellCommand;
            processInfo.RedirectStandardInput = true;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.UseShellExecute = false;

            _starMadeProcess = Process.Start(processInfo);
            _processInput = _starMadeProcess.StandardInput;

            _starMadeProcess.OutputDataReceived += ScanStarMadeOutput;
            _starMadeProcess.ErrorDataReceived += ScanStarMadeError;

            _starMadeProcess.BeginOutputReadLine();
            _starMadeProcess.BeginErrorReadLine();
        }

        /// <summary>
        /// Performs various on-start tasks, like asserting default administrators, and more.
        /// </summary>
        public void PerformStartupActions()
        {
            var defaultAdmin = SettingsService.ReadEnvironment("Default Admin", false);
            if (!string.IsNullOrEmpty(defaultAdmin))
            {
                var existingPlayer = PlayerService.GetByName(defaultAdmin);
                if (existingPlayer == null)
                    PlayerService.Create(defaultAdmin);

                PlayerService.PromoteToAdmin(defaultAdmin);
            }

            // TODO: More administrative tasks, such as running a server startup script, or more.
        }

        private void ScanStarMadeOutput(object sender, DataReceivedEventArgs e)
        {
            // Standard output does nothing.  For some reason, StarMade writes to the
            // error stream...
            // RESEARCH - Java's console.writeline writes to error stream?
        }

        private void ScanStarMadeError(object sender, DataReceivedEventArgs e)
        {
            if (ResearchMode)
                Console.WriteLine($"StarMade => {e.Data}");

            if (string.IsNullOrEmpty(e.Data))
                return;

            var parser = new ClientOutputParser(e.Data);
            var commandReactions = parser.RunCommand();
            if (!commandReactions.IsNullOrEmpty())
                SendCommands(commandReactions);

            var eventReaction = parser.ActOnServerEvent();
            if (!string.IsNullOrEmpty(eventReaction))
                SendCommand(eventReaction);
        }

        public bool RunStarMadeServer()
        {
            Console.WriteLine("Starting up StarMade...");

            // First - CD to the correct directory.
            Console.WriteLine($@"[Command]: {StarMadePathCmd}");
            _processInput.WriteLineAsync(@StarMadePathCmd);
            Thread.Sleep(100);

            // Next - run the server.
            Console.WriteLine($@"[Command]: {StarMadeRunCmd}");
            _processInput.WriteLineAsync(@StarMadeRunCmd);
            Thread.Sleep(100);

            // No issues - we have server!
            return true;
        }

        public void ExecuteServerCommand(string command)
        {
            if (command.StartsWith("/"))
            {
                SendCommand(command);
                return;
            }

            if(command.StartsWith("!"))
            {
                Console.WriteLine("! commands not yet implemented.");
                return;
            }
        }

        public void SendCommands(List<string> commands)
        {
            foreach (var command in commands)
            {
                SendCommand(command);
            }
        }

        public void SendCommand(string command)
        {
            if (_processInput == null)
                return;

            Console.WriteLine($"> {command}");
            _processInput.WriteLineAsync(command);
            Thread.Sleep(50);
        }

        public void Restart(int durationSeconds)
        {
            SendCommand($"/shutdown {durationSeconds}");
            // *2 is to double the duration in case of a long shutdown.
            // *1000 is to convert seconds to milliseconds.
            Thread.Sleep(durationSeconds * 2 * 1000);

            RunStarMadeServer();
        }

        public void Shutdown(int durationSeconds)
        {
            SendCommand($"/shutdown {durationSeconds}");
            ShouldKeepLiving = false;
        }

        public void ForceKill()
        {
            if (_starMadeProcess == null
                || _starMadeProcess.HasExited)
                return;

            // Un-set the event handlers!  Very important.
            _starMadeProcess.OutputDataReceived -= ScanStarMadeOutput;
            _starMadeProcess.ErrorDataReceived -= ScanStarMadeError;

            // Close all streams.
            _processInput.Close();

            // Dispose all streams.
            _processInput.Dispose();

            try
            {
                // Kill process if not unloaded and not exited.
                if (_starMadeProcess != null
                    && !_starMadeProcess.HasExited)
                {
                    _starMadeProcess.Kill();
                }

                // Dispose the process.
                _starMadeProcess.Dispose();
            }
            catch (NullReferenceException nrEx)
            {
                // Null reference exceptions occur when _starMadeProcess has been set
                // to null/previously disposed.  Response is: for now, swallow.
                // TODO: Implement an _isDisposed flag that's set true; if this flag
                //       is true, return from method.  Delete this catch when that's in.
            }
            catch (InvalidOperationException ioEx)
            {
                // Invalid Operation Exceptions are thrown when we try to kill a thread
                // that's already killed/disposed.  Response is: swallow and continue.
            }
            catch (Win32Exception w32Ex)
            {
                // Win32 Exceptions are due to an 'Access Denied' violation.  This usually
                // means that the StarMade process handle wasn't disposed properly, most
                // likely due to a previous failure.  Response is: for now, swallow.
            }
            finally
            {
                _processInput = null;
                _starMadeProcess = null;
                Instance = null;
            }
        }

        public void Dispose()
        {
            ForceKill();
        }

        #endregion Methods
    }
}
