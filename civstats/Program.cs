using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Threading;
using System.Data.SQLite;

namespace civstats
{
    class Program
    {
        static WebClient client;
        static SQLiteConnection demoDBConnection;

        static void Main(string[] args)
        {
            client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");

            demoDBConnection = null;

            CivFileWatcher watcher = new CivFileWatcher();
            watcher.AddWatch("civstats-1.db", UpdateDemos);

            Console.WriteLine("Reporting civ stats... press any key to exit");
            Console.ReadKey();
        }

        /**
        Parse the civstats file and send an update to the website
        */
        public static void UpdateDemos(string fullpath)
        {
            Thread.Sleep(100); // sleep a little to let the previous file IO finish
            
            if (demoDBConnection == null)
            {
                demoDBConnection = new SQLiteConnection("Data Source=" + fullpath + ";Version=3;");
            }
            
            try
            {
                demoDBConnection.Open();
                SQLiteCommand nameQuery = new SQLiteCommand("SELECT * FROM SimpleValues", demoDBConnection);
                SQLiteDataReader reader = nameQuery.ExecuteReader();

                string playerName = null;
                Demographics demos = new Demographics();
                while (reader.Read())
                {
                    string key = reader["name"].ToString();
                    string value = reader["value"].ToString();
                    if (key == "playerName")
                    {
                        playerName = value;
                    }
                    else { demos.Set(key, value); }
                }

                if (playerName != null)
                {
                    Update update = new Update(new Streamer(playerName));
                    update.demographics = demos;
                    var response = client.UploadString("http://civstats-byvkf.rhcloud.com/demos", "POST", update.ToJson());
                    Console.WriteLine(string.Format("Updated demos at {0}", DateTime.Now.ToString()));
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                demoDBConnection.Close();
            }
        }
    }

    [Serializable]
    public class Update
    {
        public Streamer streamer;
        public Demographics demographics;

        public Update(Streamer streamer)
        {
            this.streamer = streamer;
        }

        public string ToJson()
        {
            var serializer = new DataContractJsonSerializer(typeof(Update));
            using (var tempStream = new MemoryStream())
            {
                serializer.WriteObject(tempStream, this);
                return Encoding.Default.GetString(tempStream.ToArray());
            }
        }
    }

    [Serializable]
    public class Streamer
    {
        public string name;

        public Streamer(string name)
        {
            this.name = name;
        }
    }

    [Serializable]
    public class Demographics
    {
        public int population;
        public float food;
        public float production;
        public float gold;
        public int land;
        public int approval;
        public float military;
        public int literacy;

        public Demographics()
        { }

        public Demographics(int population, float food, float production, float gold, int land, int approval, int military, int literacy)
        {
            this.population = population;
            this.food = food;
            this.production = production;
            this.gold = gold;
            this.land = land;
            this.approval = approval;
            this.military = military;
            this.literacy = literacy;
        }

        public void Set(string key, string value)
        {
            switch (key)
            {
                case "population":
                    population = int.Parse(value);
                    break;
                case "food":
                    food = float.Parse(value);
                    break;
                case "production":
                    production = float.Parse(value);
                    break;
                case "gold":
                    gold = float.Parse(value);
                    break;
                case "land":
                    land = int.Parse(value);
                    break;
                case "approval":
                    approval = int.Parse(value);
                    break;
                case "military":
                    military = float.Parse(value);
                    break;
                case "literacy":
                    literacy = int.Parse(value);
                    break;
            }
        }
    }
}
