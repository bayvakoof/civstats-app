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
    }

    public class StatsTrackerEventArgs : EventArgs
    {
        public StatsTrackerEventArgs()
        { }
    }
}
