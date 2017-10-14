using System;
using System.Linq;
using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Mappers;
using Asvarduil.Penumbra.DataCore.Models;

namespace Asvarduil.Penumbra.DataCore.Repositories
{
    public class FeedbackRepository : PenumbraRepository<Feedback, FeedbackMapping>
    {
        #region Instance Accessor

        private static FeedbackRepository _instance;
        private static FeedbackRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FeedbackRepository();

                return _instance;
            }
        }

        #endregion Instance Accessor

        public static void Create(Feedback feedback)
        {
            var parameters = new Dictionary<string, object>
            {
                { "PlayerId", feedback.PlayerId },
                { "FeedbackDate", DateTime.Now },
                { "Rating", feedback.Rating },
                { "Details", feedback.Details }
            };

            Instance.RunFileExecute("Queries/AddNewFeedback.sql", parameters);
        }

        public static List<Feedback> GetByPlayerId(int playerId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "PlayerId", playerId }
            };

            var results = Instance.RunFileQuery("Queries/GetFeedbackByPlayerId.sql", parameters);
            return results.ToList();
        }

        public static List<Feedback> GetAll()
        {
            var parameters = new Dictionary<string, object>();

            var results = Instance.RunFileQuery("Queries/GetAllFeedback.sql", parameters);
            return results.ToList();
        }
    }
}
