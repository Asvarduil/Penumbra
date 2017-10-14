using System.Data.SQLite;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Extensions;

namespace Asvarduil.Penumbra.DataCore.Mappers
{
    public class FeedbackMapping : ISQLiteModelMapper<Feedback>
    {
        public Feedback MapResult(SQLiteDataReader reader)
        {
            var feedback = new Feedback
            {
                Id = reader.ReadInteger("Id"),
                PlayerId = reader.ReadInteger("PlayerId"),
                FeedbackDate = reader.ReadNullableDateTime("FeedbackDate"),
                Rating = reader.ReadInteger("Rating"),
                Details = reader.ReadString("Details")
            };

            return feedback;
        }
    }
}
