using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats.Trackers
{
    public class ReligionTracker : CivSQLiteDatabaseTracker
    {
        private List<BeliefChoice> beliefs;
        public IEnumerable<BeliefChoice> BeliefChoicesAttributes
        {
            get { return beliefs.AsEnumerable(); }
        }
        // TODO?
        // private FaithSymbol symbol; // catholicism/hinduism etc.
        // private string Name { get; private set; }

        public ReligionTracker() : base("civstats-religion")
        {
            beliefs = new List<BeliefChoice>();
        }

        protected override void ParseDatabaseEntries(Dictionary<string, string> pairs)
        {
            beliefs.Clear();

            foreach (KeyValuePair<string, string> entry in pairs)
            {
                if (entry.Key.Contains("type") || entry.Key.Contains("turn"))
                    continue; // skip the type and turn entries (e.g. <key, value>: <"1-type", "pantheon">)

                string name = entry.Value;
                BeliefTypes type;
                Enum.TryParse(pairs[entry.Key + "-type"], out type);
                int turn = int.Parse(pairs[entry.Key + "-turn"]);
                beliefs.Add(new BeliefChoice(new Belief(name, type), turn));
            }
        }
    }

    public enum BeliefTypes { Pantheon, Founder, Follower, Enhancer, Reformation }

    public class BeliefChoice
    {
        public readonly int Turn;
        private Belief belief;
        public Belief BeliefAttributes
        {
            get { return belief; }
        }

        public BeliefChoice(Belief b, int turn)
        {
            belief = b;
            Turn = turn;
        }
    }

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
