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
            // iterate over policy ids 
            // (pairs contains keys of the format, <policyId>: "1", <policyId>-turn: ... etc)
            int temp; string luaTrueVal = "1";
            var policies = pairs.Where(x => int.TryParse(x.Key, out temp));
            foreach (KeyValuePair<string, string> entry in policies)
            {
                string id = entry.Key;
                PolicyChoice choice = new PolicyChoice
                {
                    turn = int.Parse(pairs[id + "-turn"]),
                    name = pairs[id + "-name"],
                    branch = pairs[id + "-branch"]
                };
                if (entry.Value != luaTrueVal)
                    choice._destroy = true;

                update.policies.Add(choice);
            }

            return update;
        }
    }
}
