using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    [Serializable]
    class DemographicsUpdate : StatsUpdate
    {
        public List<Demographic> demographics;

        public DemographicsUpdate() : base() {
            demographics = new List<Demographic>();
        }
    }

    [Serializable]
    public class Demographic {
        public int turn;
        public string category;
        public float value;
        public float average;
        public int rank;

        public void Set(string p, string v)
        {
            switch (p)
            {
                case "turn":
                    turn = int.Parse(v);
                    break;
                case "category":
                    category = v;
                    break;
                case "value":
                    value = float.Parse(v);
                    break;
                case "average":
                    average = float.Parse(v);
                    break;
                case "rank":
                    int temp;
                    if (int.TryParse(v, out temp))
                        rank = temp;
                    break;
            }
        }
    }
}
