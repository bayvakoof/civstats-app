using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats.Trackers
{
    public class ReligionTracker : CivSQLiteDatabaseTracker
    {
        private List<Belief> beliefs;
        public IEnumerable<Belief> Beliefs
        {
            get { return beliefs.AsEnumerable(); }
        }
        // TODO?
        // private FaithSymbol symbol; // catholicism/hinduism etc.
        // private string Name { get; private set; }

        public ReligionTracker() : base("civstats-religion")
        {
            beliefs = new List<Belief>();
        }

        protected override void ParseDatabaseEntries(Dictionary<string, string> pairs)
        {
            beliefs.Clear();

            foreach (KeyValuePair<string, string> entry in pairs)
            {
                if (entry.Key.Contains("type"))
                    continue; // skip the type entries (e.g. <key, value>: <"1-type", "pantheon">)

                string name = entry.Value;
                BeliefTypes type;
                Enum.TryParse(pairs[entry.Key + "-type"], out type);
                beliefs.Add(new Belief(name, type));
            }
        }
    }

    public enum BeliefTypes { Pantheon, Founder, Follower, Enhancer, Reformation }

    public class Belief
    {
        public string Name { get; private set; }
        public BeliefTypes Type { get; private set; }

        public Belief(string name, BeliefTypes type)
        {
            Name = name;
            Type = type;
        }
    }
}
