using System.Data.SQLite;

namespace Asvarduil.Penumbra.DataCore.Mappers
{
    public interface ISQLiteModelMapper<T>
        where T : class
    {
        T MapResult(SQLiteDataReader reader);
    }
}
