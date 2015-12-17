using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats.Trackers
{
    public class GameTracker : CivSQLiteDatabaseTracker
    {
        public string Civilization { get; private set; }
        public string Map { get; private set; }
        public Speeds Speed { get; private set; }
        public Difficulties Difficulty { get; private set; }
        public MapSizes Size { get; private set; }
        public int LoadedTurn { get; private set; }
        private List<Civilization> civilizations;
        public IEnumerable<Civilization> CivilizationsAttributes // serialization
        {
            get { return civilizations.AsEnumerable(); }
        }

        public GameTracker() : base("civstats-game")
        {
            civilizations = new List<Civilization>();
        }

        /**
        The game database contains "difficulty", "speed", "map", "size", and "civilization" keys, as well as
        "[player#]-name", and "[player#]-civ" keys (e.g. "1-civ", "2-name") which give information on the other
        players and their civilizations in the game. */
        protected override void ParseDatabaseEntries(Dictionary<string, string> pairs)
        {
            // Game info
            Civilization = pairs["civilization"];
            Map = pairs["map"];
            Speeds speed;
            Enum.TryParse(pairs["speed"], out speed);
            Speed = speed;
            Difficulties diff;
            Enum.TryParse(pairs["difficulty"], out diff);
            Difficulty = diff;
            MapSizes size;
            Enum.TryParse(pairs["size"], out size);
            Size = size;

            LoadedTurn = int.Parse(pairs["loaded-turn"]);

            // Players in the game
            civilizations.Clear();

            foreach (KeyValuePair<string, string> entry in pairs)
            {
                if (Char.IsNumber(entry.Key[0]))
                {
                    // player information if the first character is a number
                    char playerNumber = entry.Key[0];
                    string civilizationName = pairs[playerNumber + "-civ"];
                    string playerName = pairs[playerNumber + "-name"];
                    if (!civilizations.Any(x => x.Leader == playerName))
                        civilizations.Add(new Civilization(civilizationName, playerName));
                }
            }
        }
    }

    public class Civilization
    {
        public readonly string Name;
        public readonly string Leader;

        public Civilization(string name, string leader)
        {
            Name = name;
            Leader = leader;
        }
    }

    public enum Speeds { Quick, Standard, Epic, Marathon };
    public enum Difficulties { Settler, Chieftain, Warlord, Prince, King, Emperor, Immortal, Diety }
    public enum MapSizes { Duel, Tiny, Small, Standard, Large, Huge }
}
