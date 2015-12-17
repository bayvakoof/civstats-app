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
        private Dictionary<string, AcquiredNaturalWonder> wonders;
        public IEnumerable<AcquiredNaturalWonder> AcquiredNaturalWonders
        {
            get { return wonders.Values.AsEnumerable(); }
        }

        public NaturalWondersTracker() : base("civstats-natural")
        {
            wonders = new Dictionary<string, AcquiredNaturalWonder>();
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
                AcquiredNaturalWonder wonder = new AcquiredNaturalWonder(
                    new NaturalWonder(pairs[id + "-name"]),
                    int.Parse(pairs[id + "-turn"]),
                    pairs[id + "-city"], 
                    int.Parse(pairs[id + "-distance"]),
                    (pairs[id + "-conquered"] == LuaTrueValue));

                wonder.Retained = (entry.Value == LuaTrueValue);
                wonders[id] = wonder; 
            }
        }
    }

    public class AcquiredNaturalWonder
    {
        private NaturalWonder naturalWonder;
        public NaturalWonder NaturalWonderAttributes
        {
            get { return naturalWonder; }
        }

        public readonly int Turn;
        public readonly string City;
        public readonly int Distance; // distance from capital
        public readonly bool Captured; // whether it was acquired thru conquest or settled
        public bool Retained { get; set; } // false if the player lost the tile

        public AcquiredNaturalWonder(NaturalWonder wonder, int turn, string city, int distance, bool captured)
        {
            naturalWonder = wonder;
            Turn = turn;
            City = city;
            Distance = distance;
            Captured = captured;
            Retained = true;
        }
    }

    public class NaturalWonder
    {
        public readonly string Name;

        public NaturalWonder(string name)
        {
            Name = name;
        }
    }
}
