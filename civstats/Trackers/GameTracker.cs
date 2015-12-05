using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace civstats.Trackers
{
    public class GameTracker : CivSQLiteDatabaseTracker
    {
        public GameInfo Info { get; private set; }
        private List<Player> players;
        public IEnumerable<Player> Players
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
            players.Clear();

            // Players
            foreach (KeyValuePair<string, string> entry in pairs)
            {
                if (Char.IsNumber(entry.Key[0]))
                {
                    // player information if the first character is a number
                    char playerNumber = entry.Key[0];
                    string name = pairs[playerNumber + "-name"];
                    string civilization = pairs[playerNumber + "-civ"];
                    if (!players.Any(x => x.Name == name))
                        players.Add(new Player(name, civilization));
                }
            }

            // Game info
            Speeds speed;
            Enum.TryParse(pairs["speed"], out speed);
            Difficulties diff;
            Enum.TryParse(pairs["difficulty"], out diff);
            MapSizes size;
            Enum.TryParse(pairs["size"], out size);

            Info = new GameInfo(pairs["civilization"], pairs["map"], speed, diff, size);
        }
    }

    public class Player
    {
        public readonly string Name;
        public readonly string Civilization;

        public Player(string name, string civilization)
        {
            Name = name;
            Civilization = civilization;
        }
    }

    public enum Speeds { Quick, Standard, Epic, Marathon };
    public enum Difficulties { Settler, Chieftain, Warlord, Prince, King, Emperor, Immortal, Diety }
    public enum MapSizes { Duel, Tiny, Small, Standard, Large, Huge }

    public class GameInfo
    {
        public readonly string Civilization;
        public readonly string Map;
        public readonly Speeds Speed;
        public readonly Difficulties Difficulty;
        public readonly MapSizes Size;

        public GameInfo(string civ, string map, Speeds speed, Difficulties diff, MapSizes size)
        {
            Civilization = civ;
            Map = map;
            Speed = speed;
            Difficulty = diff;
            Size = size;
        }
    }
}
