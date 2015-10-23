using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    interface IStatsTracker
    {
        event EventHandler<StatsTrackerEventArgs> Changed;
        StatsUpdate MakeStatsUpdate(Dictionary<string, string> pairs);
    }

    public class StatsTrackerEventArgs : EventArgs
    {
        public StatsTrackerEventArgs(StatsUpdate u)
        {
            update = u;
        }

        private StatsUpdate update;
        public StatsUpdate Update
        {
            get { return update; }
        }
    }
}
