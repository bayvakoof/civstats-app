using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats.Trackers
{
    public class WondersTracker : CivSQLiteDatabaseTracker
    {
        private const string LuaTrueValue = "1";
        private Dictionary<string, Wonder> wonders;
        public IEnumerable<Wonder> Wonders
        {
            get { return wonders.Values.AsEnumerable(); }
        }

        public WondersTracker() : base("civstats-wonders")
        {
            wonders = new Dictionary<string, Wonder>();
        }

        protected override void ParseDatabaseEntries(Dictionary<string, string> pairs)
        {
            wonders.Clear();

            // Implemented exactly like policies with keys being in the "id-parameter": true/false
            // format, iterate over wonder ids 
            int temp;
            var ids = pairs.Where(x => int.TryParse(x.Key, out temp));
            foreach (KeyValuePair<string, string> entry in ids)
            {
                string id = entry.Key;
                Wonder wonder = new Wonder(
                    int.Parse(pairs[id + "-turn"]),
                    pairs[id + "-name"], 
                    pairs[id + "-city"], 
                    pairs[id + "-conquered"] == LuaTrueValue);
                
                wonder.Retained = (entry.Value == LuaTrueValue);
                wonders[id] = wonder;
            }
        }
    }

    public class Wonder
    {
        public readonly int Turn; // the turn it was captured or built
        public readonly string Name;
        public readonly string CityName;
        public readonly bool Captured; // whether it was acquired thru conquest or built
        public bool Retained { get; set; } // false if the player lost the city containing it

        public Wonder(int turn, string name, string city, bool captured)
        {
            Turn = turn;
            Name = name;
            CityName = city;
            Captured = captured;
            Retained = true;
        }
    }
}
