using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    class ReligionTracker : CivSQLiteStatsTracker
    {
        public ReligionTracker() : base("civstats-religion")
        { }

        public override StatsUpdate MakeStatsUpdate(Dictionary<string, string> pairs)
        {
            StatsUpdate update = new StatsUpdate();
            var beliefs = pairs.ToArray().Where(x => x.Key.IndexOf("type") == -1).Select(x => x.Value);
            update.religion["beliefs"] = String.Join(", ", beliefs);
            return update;
        }
    }
}
