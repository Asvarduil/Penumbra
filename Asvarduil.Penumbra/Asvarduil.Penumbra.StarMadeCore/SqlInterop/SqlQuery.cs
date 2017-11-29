using System;

namespace Asvarduil.Penumbra.StarMadeCore
{
    public class SqlQuery
    {
        public string Command;
        public Guid GUID;
        public bool IsQuery;
        public bool IsProcessed;

        public string FullCommand => $"/* {GUID} */ {Command}";
    }
}
