using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats.Trackers
{
    public class PolicyChoicesTracker : CivSQLiteDatabaseTracker
    {
        private const string LuaTrueValue = "1";
        private Dictionary<string, PolicyChoice> choices;
        public IEnumerable<PolicyChoice> PolicyChoices
        {
            get
            {
                return choices.Values.AsEnumerable();
            }
        }

        public PolicyChoicesTracker() : base("civstats-policies")
        {
            choices = new Dictionary<string, PolicyChoice>();
        }

        /**
            Policies database contains policy choices made over the course of the game. Entries
            in the database are the policy ids, with descriptors following them. (e.g. [id]: "1", 
            [id]-turn: ... etc). Policies which have been lost from switching ideology have a 
            false value (e.g. [id]: "0")
        */
        protected override void ParseDatabaseEntries(Dictionary<string, string> pairs)
        {
            choices.Clear();

            int temp;
            var ids = pairs.Where(x => int.TryParse(x.Key, out temp));

            foreach (KeyValuePair<string, string> entry in ids)
            {
                string id = entry.Key;
                choices[entry.Key] = new PolicyChoice(
                    pairs[id + "-name"], 
                    pairs[id + "-branch"], 
                    int.Parse(pairs[id + "-turn"]),
                    float.Parse(pairs[id + "-cost"]),
                    (entry.Value == LuaTrueValue)
                );
            }
        }
    }

    public enum PolicyBranches { Tradition, Liberty, Honor, Piety, Patronage,
        Aesthetics, Commerce, Exploration, Rationalism, Freedom, Order, Autocracy }

    public class PolicyChoice
    {
        private Policy policy;
        public readonly int Turn;
        public readonly float Cost;
        // Active only applies to ideology, false if the player loses this choice 
        // after changing their ideology (i.e. is true if player has policy)
        public readonly bool Active;
        public Policy PolicyAttributes // named so for json serialization purposes
        {
            get { return policy; }
        }

        public PolicyChoice(string name, string branchName, int turn, float cost, bool active)
        {
            PolicyBranches branch;
            Enum.TryParse(branchName, out branch);
            policy = new Policy(name, branch);

            Turn = turn;
            Cost = cost;
            Active = active;
        }
    }

    public class Policy
    {
        public readonly string Name;
        public readonly PolicyBranches Branch;

        public Policy(string name, PolicyBranches branch)
        {
            Name = name;
            Branch = branch;
        }
    }
}
