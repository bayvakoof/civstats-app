using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats.Trackers
{
    public class DemographicsTracker : CivSQLiteDatabaseTracker
    {
        public int Turn { get; private set; }
        private Dictionary<string, Demographic> demographics;
        public IEnumerable<Demographic> Demographics
        {
            get { return demographics.Values.AsEnumerable(); }
        }

        public DemographicsTracker() : base("civstats-demos")
        {
            Turn = 0;
            demographics = new Dictionary<string, Demographic>();
        }

        /**
        The demographics database contains one "turn" key that contains the turn number
        for the saved demographics and the rest of the keys are in the "[category]-[property]"
        format (e.g. "food-average", "production-rank", "literacy-value") */
        protected override void ParseDatabaseEntries(Dictionary<string, string> pairs)
        {
            if (pairs.ContainsKey("turn"))
                Turn = int.Parse(pairs["turn"]);

            List<string> categories = new List<string>();
            foreach (string key in pairs.Keys)
            {
                if (key == "turn")
                    continue;

                string category = key.Split('-')[0];
                if (!categories.Contains(category))
                    categories.Add(category);
            }

            foreach (string category in categories)
            {
                Categories cat;
                Enum.TryParse(category, true, out cat);
                float val = float.Parse(pairs[category + "-value"]);
                float ave = float.Parse(pairs[category + "-average"]);
                int rank = int.Parse(pairs[category + "-rank"]);

                demographics[category] = new Demographic(cat, val, ave, rank);
            }
        }
    }

    public enum Categories { Population, Food, Production, Gold, Land,
        Military, Approval, Literacy }

    public class Demographic
    {
        public readonly Categories Category;
        public readonly float Value;
        public readonly float Average;
        public readonly int Rank;

        public Demographic(Categories category, float value, float average, int rank)
        {
            Category = category;
            Value = value;
            Average = average;
            Rank = rank;
        }
    }
}
