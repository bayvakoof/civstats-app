using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    [Serializable]
    class PoliciesUpdate : StatsUpdate
    {
        public List<PolicyChoice> policies;

        public PoliciesUpdate() : base()
        {
            policies = new List<PolicyChoice>();
        }
    }

    [Serializable]
    class PolicyChoice
    {
        public int turn;
        public int cultureCost;
        public string branch;
        public string name;
        public bool _destroy;

        public PolicyChoice()
        {
            _destroy = false;
        }
    }
}
