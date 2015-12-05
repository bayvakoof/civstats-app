using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats.Trackers
{
    public class NaturalWondersTracker : CivSQLiteDatabaseTracker
    {
        private const string LuaTrueValue = "1";
        private Dictionary<string, NaturalWonder> wonders;
        public IEnumerable<NaturalWonder> Wonders
        {
            get { return wonders.Values.AsEnumerable(); }
        }

        public NaturalWondersTracker() : base("civstats-wonders")
        {
            wonders = new Dictionary<string, NaturalWonder>();
        }

        protected override void ParseDatabaseEntries(Dictionary<string, string> pairs)
        {
            wonders.Clear(); // reread everything

            // Implemented exactly like policies with keys being in the "id-parameter": true/false
            // format, iterate over wonder ids 
            int temp;
            var ids = pairs.Where(x => int.TryParse(x.Key, out temp));
            foreach (KeyValuePair<string, string> entry in ids)
            {
                string id = entry.Key;
                NaturalWonder wonder = new NaturalWonder(
                    int.Parse(pairs[id + "-turn"]),
                    pairs[id + "-name"], 
                    pairs[id + "-city"], 
                    int.Parse(pairs[id + "-distance"]),
                    (pairs[id + "-conquered"] == LuaTrueValue));

                wonder.Retained = (entry.Value == LuaTrueValue);
                wonders[id] = wonder; 
            }
        }
    }

    public class NaturalWonder
    {
        public readonly int TurnAcquired;
        public readonly string Name;
        public readonly string CityName;
        public readonly int Distance; // distance from capital
        public readonly bool Captured; // whether it was acquired thru conquest or settled
        public bool Retained { get; set; } // false if the player lost the tile

        public NaturalWonder(int turn, string name, string city, int distance, bool captured)
        {
            TurnAcquired = turn;
            Name = name;
            CityName = city;
            Distance = distance;
            Captured = captured;
            Retained = true;
        }
    }
}
