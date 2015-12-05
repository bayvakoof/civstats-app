using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats.Trackers
{
    public class WarsTracker : CivSQLiteDatabaseTracker
    {
        private const string LuaTrueValue = "1";
        private Dictionary<string, WarEvent> warEvents;
        public IEnumerable<WarEvent> WarEvents
        {
            get { return warEvents.Values.AsEnumerable(); }
        }

        public WarsTracker() : base("civstats-wars")
        {
            warEvents = new Dictionary<string, WarEvent>();
        }

        protected override void ParseDatabaseEntries(Dictionary<string, string> pairs)
        {
            warEvents.Clear();
            foreach (KeyValuePair<string, string> entry in pairs)
            {
                string[] info = entry.Key.Split('-');
                warEvents[entry.Key] = new WarEvent(int.Parse(info[1]), info[0], info[2] != "war", entry.Value == LuaTrueValue);
            }
        }
    }
    
    public class WarEvent
    {
        public readonly int Turn;
        public readonly string Civilization;
        public readonly bool IsPeaceTreaty;
        public readonly bool Aggressor; // if war dec, then true if player initiated war, false otherwise

        public WarEvent(int turn, string civilization, bool peace, bool aggressor)
        {
            Turn = turn;
            Civilization = civilization;
            IsPeaceTreaty = peace;
            Aggressor = aggressor;
        }
    }
}
