using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Asvarduil.Penumbra.DataCore.Services;
using Asvarduil.Penumbra.DataCore.Repositories;

namespace Asvarduil.Penumbra.Tests
{
    [TestClass]
    public class DataCoreTests
    {
        [TestInitialize]
        public void OnBeforeTestsRun()
        {
            PlayerRepository.ConfigureDatabase();
        }

        [TestMethod]
        [TestCategory("Data Core")]
        public void CanSaveNewPlayer()
        {
            PlayerService.Create("UnitTest");
            var savedPlayer = PlayerService.GetByName("UnitTest");

            Assert.IsNotNull(savedPlayer);
        }

        [TestMethod]
        [TestCategory("Data Core")]
        public void CanUpdatePlayer()
        {
            // Player "already exists"...
            var initialPlayer = PlayerService.Create("UnitTest");

            var existingPlayer = PlayerService.GetByName("UnitTest");
            existingPlayer.IsAdmin = true;
            PlayerRepository.Update(existingPlayer);

            var updatedPlayer = PlayerService.GetByName("UnitTest");

            Assert.IsFalse(initialPlayer.IsAdmin == updatedPlayer.IsAdmin);
        }

        [TestMethod]
        [TestCategory("Data Core")]
        public void PlayerHasNetValue()
        {
            PlayerService.Create("UnitTest");
            var existingPlayer = PlayerService.GetByName("UnitTest");

            Assert.IsNotNull(existingPlayer.NetWorth);
            Assert.IsTrue(existingPlayer.NetWorth.Value == 0);
        }

        [TestMethod]
        [TestCategory("Data Core")]
        public void CanUpdatePlayerNetValue()
        {
            PlayerService.Create("UnitTest");
            var existingPlayer = PlayerService.GetByName("UnitTest");

            existingPlayer.NetWorth.Value = 1000000;
            PlayerService.Update(existingPlayer);

            var updatedPlayer = PlayerService.GetByName("UnitTest");

            Assert.IsFalse(updatedPlayer.NetWorth.Value != existingPlayer.NetWorth.Value);
            Assert.IsTrue(updatedPlayer.NetWorth.Value == 1000000);
        }

        [TestMethod]
        [TestCategory("Data Core")]
        public void CanPostBountyOnPlayer()
        {
            PlayerService.Create("UnitTest");
            PlayerService.Create("BobaFett");

            var bountyPoster = PlayerService.GetByName("BobaFett");
            bountyPoster.NetWorth.Value = 2000000;
            PlayerService.Update(bountyPoster);

            var postResult = PlayerService.PostBounty("BobaFett", "UnitTest", 1000000);
            Assert.IsTrue(postResult.IsSuccessful, $"Bounty post failed.  Reason: ${postResult.Message}");

            var existingPlayer = PlayerService.GetByName("UnitTest");
            var updatedBountyPoster = PlayerService.GetByName("BobaFett");

            // UnitTest should have a bounty on them...
            Assert.IsTrue(existingPlayer.Bounties.Count == 1);
            // Boba Fett should have 1000000 net worth, as he sacrificed 1m for the bounty.
            Assert.IsFalse(updatedBountyPoster.NetWorth.Value == 2000000);
        }

        [TestMethod]
        [TestCategory("Data Core")]
        public void CanClaimBountyOnPlayer()
        {
            // GIVEN: three players exist.
            PlayerService.Create("UnitTest");
            PlayerService.Create("BobaFett");
            PlayerService.Create("SamusAran");

            // GIVEN: Boba Fett is worth 2,000,000 CR
            var bountyPoster = PlayerService.GetByName("BobaFett");
            bountyPoster.NetWorth.Value = 2000000;
            PlayerService.Update(bountyPoster);

            // GIVEN: Boba Fett posts a bounty gainst Unit Test
            PlayerService.PostBounty("BobaFett", "UnitTest", 1000000);
            var bountyTarget = PlayerService.GetByName("UnitTest");

            // WHEN: Samus Aran claims the bounty against Unit Test
            PlayerService.ClaimBounty("SamusAran", "UnitTest");

            // THEN:
            // Bounties for Unit Test should be zero, as there should be no outstanding bounties.
            var defeatedBountyTarget = PlayerService.GetByName("UnitTest");
            Assert.IsTrue(defeatedBountyTarget.Bounties.Count == 0);
            Assert.IsTrue(bountyTarget.Bounties.Count > defeatedBountyTarget.Bounties.Count);
        }

        [TestMethod]
        [TestCategory("Data Core")]
        public void CanLogFeedback()
        {
            PlayerService.Create("UnitTest");
            FeedbackService.Create("UnitTest", 5, "UnitTest Feedback, yo!");

            // Fetch feedback to see if it was saved.  Normally,
            // I would do this on an as-needed basis...
            var feedbacks = FeedbackService.GetByPlayerName("UnitTest");

            Assert.IsTrue(feedbacks.Count > 0);
        }

        [TestMethod]
        [TestCategory("Data Core")]
        public void CanChangeReputation()
        {
            PlayerService.Create("UnitTest");

            const string FACTION_NAME = "Coalition of Systems";
            FactionRepository.Create(FACTION_NAME, 1);

            var operationResult = PlayerService.ChangeFactionStanding("UnitTest", FACTION_NAME, 5);
            Assert.IsTrue(operationResult.IsSuccessful, $"Could not change faction standing.  Reason: {operationResult.Message}");

            var existingPlayer = PlayerService.GetByName("UnitTest");
            Assert.IsTrue(existingPlayer.Reputations[0].Standing == 5);
        }
    }
}
