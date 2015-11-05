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
        public string branch;
        public string name;

        public PolicyChoice(string branch, string name)
        {
            this.branch = branch;
            this.name = name;
        }
    }
}
