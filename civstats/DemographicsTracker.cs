using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    class DemographicsTracker : CivSQLiteStatsTracker
    {   
        public DemographicsTracker() : base("civstats-demos")
        { }

        public override StatsUpdate MakeStatsUpdate(Dictionary<string, string> pairs)
        {
            StatsUpdate update = new StatsUpdate();

            foreach (KeyValuePair<string, string> entry in pairs)
            {
                int result;
                if (int.TryParse(entry.Value, out result))
                    update.demographics[entry.Key] = (float) result;
                else
                    update.demographics[entry.Key] = float.Parse(entry.Value);
            }

            return update;
        }
    }
}
