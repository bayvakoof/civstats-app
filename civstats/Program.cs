using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Threading;

namespace civstats
{
    class Program
    {
        static WebClient client;

        static void Main(string[] args)
        {
            client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");

            CivFileWatcher watcher = new CivFileWatcher();
            watcher.AddWatch("demographics.txt", UpdateDemos);

            Console.WriteLine("Reporting civ stats... press any key to exit");
            Console.ReadKey();
        }

        /**
        Parse the demographics.txt file and send an update to the website
        */
        public static void UpdateDemos(string fullpath)
        {
            Thread.Sleep(100); // sleep a little to let the previous file IO finish
            StreamReader reader = new StreamReader(fullpath);
            Update update = null;

            try
            {
                string playerName = null;
                do
                {
                    if (playerName == null)
                    {
                        playerName = reader.ReadLine();
                        update = new Update(new Streamer(playerName));
                        update.demographics = new Demographics();
                    }
                    else
                    {
                        string key, value;
                        string[] pair = reader.ReadLine().Split(',');
                        key = pair[0];
                        value = pair[1];
                        if (update != null)
                            update.demographics.Set(key, value);
                    }
                } while (reader.Peek() != -1);
            } catch
            { }
            finally
            {
                reader.Close();
            }
            
            try
            {
                if (update != null)
                    client.UploadString("http://civstats-byvkf.rhcloud.com/demos", "POST", update.ToJson());
                Console.WriteLine(string.Format("Updated demos at {0}", DateTime.Now.ToString()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
        public int military;
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
                    military = int.Parse(value);
                    break;
                case "literacy":
                    literacy = int.Parse(value);
                    break;
            }
        }
    }
}
