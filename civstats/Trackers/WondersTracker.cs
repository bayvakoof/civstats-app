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
        private Dictionary<string, BuiltWonder> builtWonders;
        public IEnumerable<BuiltWonder> BuiltWonders
        {
            get { return builtWonders.Values.AsEnumerable(); }
        }

        public WondersTracker() : base("civstats-wonders")
        {
            builtWonders = new Dictionary<string, BuiltWonder>();
        }

        protected override void ParseDatabaseEntries(Dictionary<string, string> pairs)
        {
            builtWonders.Clear();

            // Implemented exactly like policies with keys being in the "id-parameter": true/false
            // format, iterate over wonder ids 
            int temp;
            var ids = pairs.Where(x => int.TryParse(x.Key, out temp));
            foreach (KeyValuePair<string, string> entry in ids)
            {
                string id = entry.Key;

                BuiltWonder builtWonder = new BuiltWonder(
                    new Wonder(pairs[id + "-name"]),
                    int.Parse(pairs[id + "-turn"]),
                    pairs[id + "-city"], 
                    pairs[id + "-conquered"] == LuaTrueValue);

                builtWonder.Retained = (entry.Value == LuaTrueValue);
                builtWonders[id] = builtWonder;
            }
        }
    }

    public class BuiltWonder
    {
        public readonly int Turn; // the turn it was captured or built

        private Wonder wonder;
        public Wonder WonderAttributes
        {
            get { return wonder; }
        }

        public readonly string City;
        public readonly bool Captured; // whether it was acquired thru conquest or built
        public bool Retained { get; set; } // false if the player lost the city containing it

        public BuiltWonder(Wonder w, int turn, string city, bool captured)
        {
            wonder = w;
            Turn = turn;
            City = city;
            Captured = captured;
            Retained = true;
        }
    }

    public class Wonder
    {
        public readonly string Name;

        public Wonder(string name)
        {
            Name = name;
        }
    }
}
