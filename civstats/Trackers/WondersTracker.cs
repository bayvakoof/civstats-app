using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    public class WondersTracker : CivSQLiteStatsTracker
    {
        public WondersTracker() : base("civstats-wonders")
        { }

        public override StatsUpdate MakeStatsUpdate(Dictionary<string, string> pairs)
        {
            // Implemented exactly like policies with keys being in the  "id-parameter": true/false
            // format
            WondersUpdate update = new WondersUpdate();
            // iterate over wonder ids 
            int temp; string luaTrueVal = "1";
            var wonders = pairs.Where(x => int.TryParse(x.Key, out temp));
            foreach (KeyValuePair<string, string> entry in wonders)
            {
                string id = entry.Key;
                Wonder wonder = new Wonder
                {
                    name = pairs[id + "-name"],
                    city = pairs[id + "-city"],
                    turn = int.Parse(pairs[id + "-turn"]),
                    conquered = (pairs[id + "-conquered"] == luaTrueVal) ? true : false
                };
                if (entry.Value != luaTrueVal)
                    wonder._destroy = true;

                update.wonders.Add(wonder);
            }

            return update;
        }
    }
}
