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
            WondersUpdate update = new WondersUpdate();
            foreach (KeyValuePair<string, string> entry in pairs)
            {
                update.wonders.Add( new Wonder(entry.Key, int.Parse(entry.Value)) );
            }

            return update;
        }
    }
}
