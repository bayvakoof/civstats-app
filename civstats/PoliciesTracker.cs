using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    class PoliciesTracker : CivSQLiteStatsTracker
    {
        public PoliciesTracker() : base("civstats-policies")
        { }

        public override StatsUpdate MakeStatsUpdate(Dictionary<string, string> pairs)
        {
            StatsUpdate update = new StatsUpdate();

            foreach (KeyValuePair<string, string> entry in pairs)
            {
                update.policies[entry.Key] = int.Parse(entry.Value);
            }

            return update;
        }
    }
}
