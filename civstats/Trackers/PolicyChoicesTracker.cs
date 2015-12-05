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
        public IEnumerable<PolicyChoice> Choices
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
                PolicyBranches branch;
                Enum.TryParse(pairs[id + "-branch"], out branch);
                choices[entry.Key] = new PolicyChoice
                {
                    Turn = int.Parse(pairs[id + "-turn"]),
                    Cost = float.Parse(pairs[id + "-cost"]),
                    Branch = branch,
                    Name = pairs[id + "-name"],
                    Active = entry.Value == LuaTrueValue
                };
            }
        }
    }

    public enum PolicyBranches { Tradition, Liberty, Honor, Piety, Patronage,
        Aesthetics, Commerce, Exploration, Rationalism, Freedom, Order, Autocracy }

    public class PolicyChoice
    {
        public int Turn { get; set; }
        public float Cost { get; set; }
        public PolicyBranches Branch { get; set; }
        public string Name { get; set; }
        // Active only applies to ideology, false if the player loses this choice 
        // after changing their ideology (i.e. is true if player has policy)
        public bool Active { get; set; } 
    }
}
