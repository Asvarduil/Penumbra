using System;
using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Repositories;

namespace Asvarduil.Penumbra.DataCore.Services
{
    public class FeedbackService
    {
        public static void Create(string playerName, int rating, string details)
        {
            var player = PlayerService.GetByName(playerName);
            if (player == null)
                return;

            var feedback = new Feedback
            {
                PlayerId = player.Id,
                FeedbackDate = DateTime.Now,
                Rating = rating,
                Details = details
            };

            FeedbackRepository.Create(feedback);
        }

        public static List<Feedback> GetByPlayerId(int playerId)
        {
            return FeedbackRepository.GetByPlayerId(playerId);
        }

        public static List<Feedback> GetByPlayerName(string playerName)
        {
            var player = PlayerService.GetByName(playerName);
            if (player == null)
                return null;

            return GetByPlayerId(player.Id);
        }

        public static List<Feedback> GetAll()
        {
            return FeedbackRepository.GetAll();
        }
    }
}
