using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    public class DemographicsTracker : CivSQLiteStatsTracker
    {   
        public DemographicsTracker() : base("civstats-demos")
        { }

        public override StatsUpdate MakeStatsUpdate(Dictionary<string, string> pairs)
        {
            DemographicsUpdate update = new DemographicsUpdate();
            int turn = 0;

            foreach (KeyValuePair<string, string> entry in pairs)
            {
                if (entry.Key == "turn")
                {
                    turn = int.Parse(entry.Value);
                    continue;
                }

                // key is of the format <category>-<property>
                // e.g. food-average, production-rank, literacy-value etc.
                string[] descriptors = entry.Key.Split('-');
                string category = descriptors[0], property = descriptors[1];

                var demo = update.demographics.Find(x => x.category == category);

                if (demo == null)
                {
                    demo = new Demographic();
                    demo.category = category;
                    demo.turn = turn;
                    update.demographics.Add(demo);
                }

                demo.Set(property, entry.Value);
            }
            
            return update;
        }
    }
}
