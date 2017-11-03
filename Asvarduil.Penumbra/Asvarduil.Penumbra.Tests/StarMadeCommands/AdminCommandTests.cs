using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Asvarduil.Penumbra.StarMadeCore;
using Asvarduil.Penumbra.DataCore.Repositories;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.Tests.StarMadeCommands
{
    [TestClass]
    public class AdminCommandTests
    {
        [TestInitialize]
        public void OnBeforeTestsRun()
        {
            PlayerRepository.ConfigureDatabase();
        }

        [TestMethod]
        [TestCategory("Admin Commands")]
        public void AdminCanPromoteAnotherAdmin()
        {
            PlayerService.Create("SuperUser");
            PlayerService.Create("UnitTest");

            PlayerService.PromoteToAdmin("SuperUser");

            string serverOutput = "[CHANNELROUTER] RECEIVED MESSAGE ON Server(0): [CHAT][sender=SuperUser][receiverType=CHANNEL][receiver=Server][message=!PROMOTEADMIN UnitTest]";
            var expectedResult = new List<string>
            {
                "/server_message_broadcast info \"UnitTest is now a MicroMade Administrator!\""
            };
            TestCommandOutput(serverOutput, expectedResult);

            var existingPlayer = PlayerService.GetByName("UnitTest");
            Assert.IsTrue(existingPlayer.IsAdmin);
        }

        [TestMethod]
        [TestCategory("Admin Commands")]
        public void AdminCanDemoteAnotherAdmin()
        {
            PlayerService.Create("SuperUser");
            PlayerService.Create("UnitTest");

            PlayerService.PromoteToAdmin("SuperUser");
            PlayerService.PromoteToAdmin("UnitTest");

            string serverOutput = "[CHANNELROUTER] RECEIVED MESSAGE ON Server(0): [CHAT][sender=SuperUser][receiverType=CHANNEL][receiver=Server][message=!DEMOTEADMIN UnitTest]";
            var expectedResult = new List<string>
            {
                "/server_message_broadcast info \"UnitTest is no longer a MicroMade Administrator.\""
            };
            TestCommandOutput(serverOutput, expectedResult);

            var existingPlayer = PlayerService.GetByName("UnitTest");
            Assert.IsFalse(existingPlayer.IsAdmin);
        }

        [TestMethod]
        [TestCategory("Admin Commands")]
        public void AdminCanClearBountiesOnPlayer()
        {
            PlayerService.Create("SuperUser");
            PlayerService.Create("UnitTest");
            PlayerService.Create("Boba_Fett");

            PlayerService.PromoteToAdmin("SuperUser");

            var postingPlayer = PlayerService.GetByName("Boba_Fett");
            postingPlayer.NetWorth.Value = 1000000;
            PlayerService.Update(postingPlayer);

            PlayerService.PostBounty("Boba_Fett", "UnitTest", 500000);

            string serverOutput = "[CHANNELROUTER] RECEIVED MESSAGE ON Server(0): [CHAT][sender=SuperUser][receiverType=CHANNEL][receiver=Server][message=!REMOVEBOUNTY UnitTest]";
            var expectedResult = new List<string>
            {
                "/server_message_broadcast info \"It has been decreed that UnitTest no longer has a bounty on them.\""
            };
            TestCommandOutput(serverOutput, expectedResult);
        }

        [TestMethod]
        [TestCategory("Admin Commands")]
        public void AdminCanGrantNetWorthToPlayer()
        {
            PlayerService.Create("SuperUser");
            PlayerService.Create("UnitTest");

            PlayerService.PromoteToAdmin("SuperUser");

            string serverOutput = "[CHANNELROUTER] RECEIVED MESSAGE ON Server(0): [CHAT][sender=SuperUser][receiverType=CHANNEL][receiver=Server][message=!GRANTNETWORTH UnitTest 1000000]";
            var expectedResult = new List<string>
            {
                "/server_message_to info UnitTest \"You have had a personal value of 1000000 Credits bestowed upon you.  Use !NETWORTH ME to check your new net worth.\""
            };
            TestCommandOutput(serverOutput, expectedResult);

            var existingPlayer = PlayerService.GetByName("UnitTest");
            Assert.IsTrue(existingPlayer.NetWorth.Value == 1000000);
        }

        [TestMethod]
        [TestCategory("Admin Commands")]
        public void AdminCanReadSubmittedFeedback()
        {
            PlayerService.Create("SuperUser");
            PlayerService.Create("UnitTest");
            PlayerService.Create("Boba_Fett");

            PlayerService.PromoteToAdmin("SuperUser");

            FeedbackService.Create("UnitTest", 5, "This server is awesome!");
            FeedbackService.Create("Boba_Fett", 3, "UnitTest is a n00b LOL!");

            string serverOutput = "[CHANNELROUTER] RECEIVED MESSAGE ON Server(0): [CHAT][sender=SuperUser][receiverType=CHANNEL][receiver=Server][message=!READFEEDBACK]";
            var expectedResult = new List<string>
            {
                "/server_message_to info SuperUser \"Rating: 5 - Details: This server is awesome!\"",
                "/server_message_to info SuperUser \"Rating: 3 - Details: UnitTest is a n00b LOL!\""
            };
            TestCommandOutput(serverOutput, expectedResult);
        }

        [TestMethod]
        [TestCategory("Admin Commands")]
        public void AdminCanDespawnShips()
        {
            PlayerService.Create("SuperUser");

            PlayerService.PromoteToAdmin("SuperUser");

            string serverOutput = "[CHANNELROUTER] RECEIVED MESSAGE ON Server(0): [CHAT][sender=SuperUser][receiverType=CHANNEL][receiver=Server][message=!REMOVESHIP Starship Taco]";
            var expectedResult = new List<string>
            {
                "/despawn_all \"Starship Taco\" all true",
                "/server_message_to info SuperUser \"Removed all ships that start with Starship Taco.\""
            };

            TestCommandOutput(serverOutput, expectedResult);
        }

        /// <summary>
        /// Given a server output, ensure that when the appropriate command is run, that the expected result is generated.
        /// </summary>
        /// <param name="serverOutput">Server output line to parse</param>
        /// <param name="expectedResult">Expected result of the output parser running the command.</param>
        private void TestCommandOutput(string serverOutput, List<string> expectedResults)
        {
            var parser = new ClientOutputParser(serverOutput);
            List<string> reactions = parser.RunCommand();
            foreach (var reaction in reactions)
            {
                string errorMessage = $"Reaction <{reaction}> was not present in the list of expected results.";
                Assert.IsTrue(expectedResults.Any(e => e == reaction), errorMessage);
            }
        }
    }
}
