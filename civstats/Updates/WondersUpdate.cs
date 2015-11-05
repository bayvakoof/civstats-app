using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    [Serializable]
    public class WondersUpdate : StatsUpdate
    {
        public List<Wonder> wonders;

        public WondersUpdate() : base()
        {
            wonders = new List<Wonder>();
        }
    }

    [Serializable]
    public class Wonder
    {
        string name;
        int acquired; // turn acquired

        public Wonder(string name, int acquired)
        {
            this.name = name;
            this.acquired = acquired;
        }
    }
}
