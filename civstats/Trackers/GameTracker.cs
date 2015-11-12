using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    public class GameTracker : CivSQLiteStatsTracker
    {
        public GameTracker() : base("civstats-game")
        { }

        public override StatsUpdate MakeStatsUpdate(Dictionary<string, string> pairs)
        {
            return new GameUpdate { gameinfo = pairs };
        }
    }
}
