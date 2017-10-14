using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Asvarduil.Penumbra.StarMadeCore;
using Asvarduil.Penumbra.DataCore.Services;
using Asvarduil.Penumbra.DataCore.Repositories;

namespace Asvarduil.Penumbra.Tests.StarMadeCommands
{
    [TestClass]
    public class ChatCommandTests
    {
        [TestInitialize]
        public void OnBeforeTestsRun()
        {
            PlayerRepository.ConfigureDatabase();
        }

        [TestMethod]
        [TestCategory("Chat Commands")]
        public void CanIssueFeedback()
        {
            PlayerService.Create("UnitTest");
            var existingPlayer = PlayerService.GetByName("UnitTest");

            string serverOutput = "[CHANNELROUTER] RECEIVED MESSAGE ON Server(0): [CHAT][sender=UnitTest][receiverType=CHANNEL][receiver=Server][message=!FEEDBACK 5 Test message]";
            var expectedResult = new List<string>
            {
                "/server_message_to info UnitTest \"Your feedback has been received.  Thanks for telling us about what's going on!\""
            };
            TestCommandOutput(serverOutput, expectedResult);

            var feedbacks = FeedbackService.GetAll();
            Assert.IsTrue(feedbacks.Count == 1);

            var createdFeedback = feedbacks[0];
            Assert.IsTrue(createdFeedback.PlayerId == existingPlayer.Id);
            Assert.IsTrue(createdFeedback.Rating == 5);
            Assert.IsTrue(createdFeedback.Details == "TEST MESSAGE");
        }

        [TestMethod]
        [TestCategory("Chat Commands")]
        public void CanViewNetWorth()
        {
            PlayerService.Create("UnitTest");
            var existingPlayer = PlayerService.GetByName("UnitTest");

            existingPlayer.NetWorth.Value = 1000000;
            PlayerService.Update(existingPlayer);

            var serverOutput = "[CHANNELROUTER] RECEIVED MESSAGE ON Server(0): [CHAT][sender=UnitTest][receiverType=CHANNEL][receiver=Server][message=!NETWORTH ME]";
            var expectedResult = new List<string>
            {
                "/server_message_to info UnitTest \"Your net worth is 1000000.  You can withdraw this by using !CASHOUT.\""
            };
            TestCommandOutput(serverOutput, expectedResult);
        }

        [TestMethod]
        [TestCategory("Chat Commands")]
        public void CanCashoutNetWorth()
        {
            PlayerService.Create("UnitTest");
            var existingPlayer = PlayerService.GetByName("UnitTest");

            existingPlayer.NetWorth.Value = 1000000;
            PlayerService.Update(existingPlayer);

            var serverOutput = "[CHANNELROUTER] RECEIVED MESSAGE ON Server(0): [CHAT][sender=UnitTest][receiverType=CHANNEL][receiver=Server][message=!CASHOUT]";
            var expectedResult = new List<string>
            {
                "/give_credits UnitTest 1000000"
            };
            TestCommandOutput(serverOutput, expectedResult);
        }

        [TestMethod]
        [TestCategory("Chat Commands")]
        public void CanCreateBountyViaChat()
        {
            PlayerService.Create("Boba_Fett");
            PlayerService.Create("UnitTest");
            var existingPlayer = PlayerService.GetByName("UnitTest");

            existingPlayer.NetWorth.Value = 1000000;
            PlayerService.Update(existingPlayer);

            var serverOutput = "[CHANNELROUTER] RECEIVED MESSAGE ON Server(0): [CHAT][sender=UnitTest][receiverType=CHANNEL][receiver=Server][message=!BOUNTY Boba_Fett 500000]";
            var expectedResult = new List<string>
            {
                "/server_message_broadcast info \"A bounty has been posted for Boba_Fett!\""
            };
            TestCommandOutput(serverOutput, expectedResult);

            // Boba Fett should have a bounty on him...
            var targetPlayer = PlayerService.GetByName("Boba_Fett");
            Assert.IsTrue(targetPlayer.Bounties.Count == 1);
            Assert.IsTrue(targetPlayer.Bounties[0].Value == 500000);

            // Unit Test should be down to half of their original wealth.
            var sourcePlayer = PlayerService.GetByName("UnitTest");
            Assert.IsTrue(sourcePlayer.NetWorth.Value == 500000);
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
