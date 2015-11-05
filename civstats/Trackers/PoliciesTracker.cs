using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    public class PoliciesTracker : CivSQLiteStatsTracker
    {
        public PoliciesTracker() : base("civstats-policies")
        { }

        public override StatsUpdate MakeStatsUpdate(Dictionary<string, string> pairs)
        {
            PoliciesUpdate update = new PoliciesUpdate();
            foreach (KeyValuePair<string, string> entry in pairs)
            {
                if (entry.Key == "turn")
                {
                    update.turn = int.Parse(entry.Value);
                    continue;
                }

                string branch = entry.Key.Split('-')[0];
                if (!string.IsNullOrEmpty(entry.Value))
                    update.policies.Add(new PolicyChoice(branch, entry.Value));
            }

            return update;
        }
    }
}
