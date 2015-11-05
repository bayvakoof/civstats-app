using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    [Serializable]
    public class ReligionUpdate : StatsUpdate
    {
        // public string symbol;
        public List<Belief> beliefs;

        public ReligionUpdate() : base()
        {
            beliefs = new List<Belief>();
        }
    }

    [Serializable]
    public class Belief
    {
        public string name;
        public string type;

        public Belief(string name, string type)
        {
            this.name = name;
            this.type = type;
        }
    }
}
