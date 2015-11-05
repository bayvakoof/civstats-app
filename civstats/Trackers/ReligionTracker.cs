using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    public class ReligionTracker : CivSQLiteStatsTracker
    {
        public ReligionTracker() : base("civstats-religion")
        { }

        public override StatsUpdate MakeStatsUpdate(Dictionary<string, string> pairs)
        {
            ReligionUpdate update = new ReligionUpdate();

            foreach (KeyValuePair<string, string> entry in pairs)
            {
                if (entry.Key.Contains("type"))
                    continue; // skip the type entries (e.g. <key, value>: <"1-type", "pantheon">)

                string name = entry.Value;
                string type = pairs[entry.Key + "-type"];
                update.beliefs.Add(new Belief(name, type));
            }
            
            return update;
        }
    }
}
