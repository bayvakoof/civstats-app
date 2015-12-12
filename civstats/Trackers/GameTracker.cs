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
        private List<Player> players;
        public IEnumerable<Player> CivilizationAttributes // serialization
        {
            get { return players.AsEnumerable(); }
        }

        public GameTracker() : base("civstats-game")
        {
            players = new List<Player>();
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

            // Players
            players.Clear();

            foreach (KeyValuePair<string, string> entry in pairs)
            {
                if (Char.IsNumber(entry.Key[0]))
                {
                    // player information if the first character is a number
                    char playerNumber = entry.Key[0];
                    string name = pairs[playerNumber + "-name"];
                    string civilization = pairs[playerNumber + "-civ"];
                    if (!players.Any(x => x.Leader == name))
                        players.Add(new Player(name, civilization));
                }
            }
        }
    }

    public class Player
    {
        public readonly string Leader;
        public readonly string Civilization;

        public Player(string leader, string civilization)
        {
            Leader = leader;
            Civilization = civilization;
        }
    }

    public enum Speeds { Quick, Standard, Epic, Marathon };
    public enum Difficulties { Settler, Chieftain, Warlord, Prince, King, Emperor, Immortal, Diety }
    public enum MapSizes { Duel, Tiny, Small, Standard, Large, Huge }
}
