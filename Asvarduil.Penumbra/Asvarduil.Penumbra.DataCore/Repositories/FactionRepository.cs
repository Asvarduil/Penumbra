using System.Linq;
using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Mappers;

namespace Asvarduil.Penumbra.DataCore.Repositories
{
    public class FactionRepository : PenumbraRepository<Faction, FactionMapper>
    {
        #region Instance

        private static FactionRepository _instance;
        private static FactionRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FactionRepository();

                return _instance;
            }
        }

        #endregion Instance

        #region Faction Cache

        // Programmer's Note:
        // ------------------
        // Factions are very lightweight, and are immutable save for changes by the
        // server operator.  Thus, to help with performance, we can load these all
        // the first time they're needed, and cache them in memory for subsequent
        // calls, thus improving performance.

        private static List<Faction> _factions;

        public static Faction GetById(int factionId)
        {
            if (_factions == null)
                _factions = GetAll();

            return _factions.FirstOrDefault(f => f.Id == factionId);
        }

        public static Faction GetByName(string factionName)
        {
            if (_factions == null)
                _factions = GetAll();

            return _factions.FirstOrDefault(f => f.Name == factionName);
        }

        #endregion Faction Cache

        #region Accessors

        public static List<Faction> GetAll()
        {
            return Instance.RunFileQuery("Queries/GetAllFactions.sql", null).ToList();
        }

        #endregion Accessors
    }
}
