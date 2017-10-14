using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Asvarduil.Penumbra.DataCore.Repositories;
using Asvarduil.Penumbra.StarMadeCore;

namespace Asvarduil.Penumbra.Tests.StarMadeCommands
{
    [TestClass]
    public class ClientCommanderTests
    {
        [TestInitialize]
        public void OnBeforeTestsRun()
        {
            PlayerRepository.ConfigureDatabase();
        }

        //[TestMethod]
        public void CanOpenAndCloseStarMade()
        {
            using (var commander = new ClientCommander())
            {
                commander.OpenCommandPrompt();
                bool serverStarted = commander.RunStarMadeServer();
                Assert.IsTrue(serverStarted);

                // Allow it to run for about two minutes,
                // to be sure that the game has started up
                // and will accept commands.
                Thread.Sleep(120000);

                commander.Shutdown(30);

                Assert.IsFalse(commander.IsClientAlive);
            }
        }
    }
}
