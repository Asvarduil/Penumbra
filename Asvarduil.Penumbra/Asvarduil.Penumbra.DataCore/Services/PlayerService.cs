using System;
using System.Linq;
using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Repositories;
using Asvarduil.Penumbra.DataCore.Models;

namespace Asvarduil.Penumbra.DataCore.Services
{
    /// <summary>
    /// Provides simplified means by which to run player interactions.
    /// </summary>
    public static class PlayerService
    {
        /// <summary>
        /// Creates a new player record based on a player name.
        /// </summary>
        /// <param name="name">Name of the player to create</param>
        /// <returns>Player record.</returns>
        public static Player Create(string name)
        {
            // Initialize the new player.
            var createDate = DateTime.Now;
            var player = new Player
            {
                Name = name,
                JoinDate = createDate,
                LastLoggedInDate = createDate
            };

            PlayerRepository.Create(player);
            var createdPlayer = PlayerRepository.GetByName(player.Name);

            // Initialize the new player's Net Worth
            var netWorth = new NetWorth
            {
                PlayerId = createdPlayer.Id,
                Value = 0,
                LastUpdatedDate = createDate
            };

            NetWorthRepository.Create(netWorth);
            var createdNetWorth = NetWorthRepository.GetByPlayerId(createdPlayer.Id);

            // Reputations will be initialized the first time they change.

            // Build the player.
            createdPlayer.NetWorth = createdNetWorth;
            createdPlayer.Bounties = new List<Bounty>();

            return createdPlayer;
        }

        /// <summary>
        /// Gets an existing player record by name.
        /// </summary>
        /// <param name="name">Name to look for.</param>
        /// <returns>Player if found, otherwise null.</returns>
        public static Player GetByName(string name)
        {
            var existingPlayer = PlayerRepository.GetByName(name);
            if (existingPlayer == null)
                return null;

            var existingNetWorth = NetWorthRepository.GetByPlayerId(existingPlayer.Id);
            if (existingNetWorth == null)
                throw new ApplicationException($"Could not find a NetWorths record for Player ID #{existingPlayer.Id}");

            var existingBounties = BountyRepository.GetByTargetPlayerId(existingPlayer.Id);
            if (existingBounties == null)
                existingBounties = new List<Bounty>();

            var existingReputations = ReputationRepository.GetByPlayerId(existingPlayer.Id);
            if (existingReputations == null)
                existingReputations = new List<Reputation>();

            existingPlayer.NetWorth = existingNetWorth;
            existingPlayer.Bounties = existingBounties;
            existingPlayer.Reputations = existingReputations;

            return existingPlayer;
        }

        /// <summary>
        /// Updates a player record from the in-memory object.
        /// </summary>
        /// <param name="player">Player to update</param>
        public static void Update(Player player)
        {
            PlayerRepository.Update(player);
            NetWorthRepository.Update(player.NetWorth);
        }

        /// <summary>
        /// Raises the Net Worth for the given player by the given amount.
        /// </summary>
        /// <param name="playerName">Player to grant net worth to</param>
        /// <param name="amount">Amount of net worth the player gets.</param>
        public static void IncreaseNetWorth(string playerName, int amount)
        {
            var player = GetByName(playerName);
            if (player == null)
                return;

            player.NetWorth.Value += amount;
            if (player.NetWorth.Value < 0)
                player.NetWorth.Value = 0;

            player.NetWorth.LastUpdatedDate = DateTime.Now;
            Update(player);
        }

        /// <summary>
        /// Posts a bounty from one player to another
        /// </summary>
        /// <param name="postingPlayerName">Name of the player posting the bounty</param>
        /// <param name="targetPlayerName">Player who will have the bounty posted against them.</param>
        /// <param name="bountyValue">Value of the bounty on the target</param>
        /// <returns>BountyPostResult, detailing success of the operation, or if it failed, why.</returns>
        public static OperationResult PostBounty(string postingPlayerName, string targetPlayerName, int bountyValue)
        {
            var result = new OperationResult();

            var postingPlayer = GetByName(postingPlayerName);
            if(postingPlayer == null)
            {
                result.Message = $"Posting player ${postingPlayerName} does not exist.";
                return result;
            }

            var targetPlayer = GetByName(targetPlayerName);
            if(targetPlayer == null)
            {
                result.Message = $"Target player ${targetPlayerName} does not exist.";
                return result;
            }

            if(postingPlayer.NetWorth.Value < bountyValue)
            {
                result.Message = $"You don't have enough Net Worth to back the bounty on ${targetPlayerName}.  Use !NETWORTH ME to see your accumulated value.";
                return result;
            }

            postingPlayer.NetWorth.Value -= bountyValue;
            Update(postingPlayer);

            var bounty = new Bounty
            {
                TargetPlayerId = targetPlayer.Id,
                PostedDate = DateTime.Now,
                Value = bountyValue
            };

            BountyRepository.Create(bounty);

            return result;
        }

        /// <summary>
        /// Claims a bounty, with credit going to the claiming player.
        /// </summary>
        /// <param name="claimingPlayerName">Name of the player claiming the bounty.</param>
        /// <param name="targetPlayerName">Name of the player against whom bounties were claimed.</param>
        /// <returns>BountyPostResult, detailing success of the operation, or if it failed, why.</returns>
        public static OperationResult ClaimBounty(string claimingPlayerName, string targetPlayerName)
        {
            var result = new OperationResult();

            var claimingPlayer = GetByName(claimingPlayerName);
            if(claimingPlayer == null)
            {
                result.Message = $"Player {claimingPlayerName} doesn't exist.";
                return result;
            }

            var targetPlayer = GetByName(targetPlayerName);
            if(targetPlayer == null)
            {
                result.Message = $"Player {targetPlayerName} doesn't exist.";
                return result;
            }

            BountyRepository.UpdateWithClaim(targetPlayer.Id, claimingPlayer.Id);

            return result;
        }

        /// <summary>
        /// Removes all bounties from the given player.
        /// </summary>
        /// <param name="targetPlayerName">Player from whom to remove bounties.</param>
        /// <returns>BountyPostResult, detailing success of the operation, or if it failed, why.</returns>
        public static OperationResult RemoveBounty(string targetPlayerName)
        {
            var result = new OperationResult();

            var targetPlayer = GetByName(targetPlayerName);
            if(targetPlayer == null)
            {
                result.Message = $"Player {targetPlayerName} doesn't exist.";
                return result;
            }

            BountyRepository.RemoveAllForPlayer(targetPlayer.Id);

            return result;
        }

        /// <summary>
        /// Determine if the named player is an administrator.
        /// </summary>
        /// <param name="playerName">Player to check</param>
        /// <returns>False if the player doesn't exist or isn't an admin, otherwise true.</returns>
        public static bool IsAdmin(string playerName)
        {
            var player = PlayerRepository.GetByName(playerName);
            if (player == null)
                return false;

            return player.IsAdmin;
        }

        /// <summary>
        /// Promotes the given player to administrator.
        /// </summary>
        /// <param name="playerName">Player to promote</param>
        public static void PromoteToAdmin(string playerName)
        {
            var player = GetByName(playerName);
            if (player == null)
                return;

            if (player.IsAdmin)
                return;

            player.IsAdmin = true;
            Update(player);
        }

        /// <summary>
        /// Demote an admin back to a normal user.
        /// </summary>
        /// <param name="playerName">Player to demote</param>
        public static void DemoteFromAdmin(string playerName)
        {
            var player = GetByName(playerName);
            if (player == null)
                return;

            if (!player.IsAdmin)
                return;

            player.IsAdmin = false;
            Update(player);
        }

        /// <summary>
        /// Gets all logged on players.
        /// </summary>
        /// <returns>All logged-on players.</returns>
        public static List<Player> GetLoggedOnPlayers()
        {
            var result = PlayerRepository.GetLoggedOnPlayers();
            return result;
        }

        /// <summary>
        /// Adds a certain amount of fame (or disrepute) for a player, to their relationship with a faction.
        /// </summary>
        /// <param name="playerName">Name of the player to change reputation for</param>
        /// <param name="factionName">Faction with which reputation will be changed</param>
        /// <param name="amount">Amount by which reputation will change</param>
        public static OperationResult ChangeFactionStanding(string playerName, string factionName, int amount)
        {
            var result = new OperationResult();

            var player = GetByName(playerName);
            if (player == null)
            {
                result.Message = $"Player {playerName} has no entry in the Penumbra database.";
                return result;
            }

            var faction = FactionRepository.GetByName(factionName);
            if (faction == null)
            {
                result.Message = $"Faction {factionName} has no entry in the Penumbra database.";
                return result;
            }

            var relevantReputation = player.Reputations.FirstOrDefault(r => r.Id == faction.Id);
            if (relevantReputation == null)
            {
                relevantReputation = new Reputation
                {
                    PlayerId = player.Id,
                    FactionId = faction.Id,
                    Standing = amount
                };

                ReputationRepository.Create(relevantReputation);
            }
            else
            {
                relevantReputation.Standing += amount;

                ReputationRepository.Update(relevantReputation);
            }

            return result;
        }
    }
}
