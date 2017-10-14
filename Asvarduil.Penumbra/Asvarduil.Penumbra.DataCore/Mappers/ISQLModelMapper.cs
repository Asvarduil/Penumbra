using System.Data.SqlClient;

namespace Asvarduil.Penumbra.DataCore.Mappers
{
    public interface ISQLModelMapper<T>
        where T : class
    {
        T MapResult(SqlDataReader reader);
    }
}
