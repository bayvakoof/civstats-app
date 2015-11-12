using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    [Serializable]
    class GameUpdate : StatsUpdate
    {
        public Dictionary<string, string> gameinfo;
    }
}
