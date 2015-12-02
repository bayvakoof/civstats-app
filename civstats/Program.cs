using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using civstats.Trackers;

namespace civstats
{
    class Program
    {
        static string id;
        static string key;
        const string SITE_URL = "http://civstats-byvkf.rhcloud.com/";
#if DEBUG
        const int API_VERSION = int.MaxValue;
#else
        const int API_VERSION = 1; // the version of the API the app is compatible with
#endif

        static void Main(string[] args)
        {
            Console.Title = "CivStats";
            if (!IsUpToDate())
            {
                Console.Write("This app needs to be updated. Please update the app.");
                Console.ReadKey();
                return;
            }
            
            CheckSettings();

            id = Properties.Settings.Default.id;
            key = Properties.Settings.Default.key;

            IStatsTracker[] trackers = {
                new GameTracker(),
                new DemographicsTracker(),
                new PolicyChoicesTracker(),
                new ReligionTracker(),
                new WondersTracker(),
                new WarsTracker(),
                new NaturalWondersTracker()
            };

            foreach (IStatsTracker tracker in trackers)
            {
                tracker.Changed += StatsTrackerChangedHandler;
            }

            Console.WriteLine("Reporting civ stats. Please exit after you've finished playing.");
            Console.ReadKey();
        }
        
        static void StatsTrackerChangedHandler(object source, StatsTrackerEventArgs e)
        {
            Uri uploadUri = new Uri(SITE_URL + "players/" + id + "/update");
            WebClient client = new WebClient();
            IStatsTracker tracker = (IStatsTracker)source;
            tracker.GetType();
            //Update update = tracker.GetUpdate();
            //client.UploadString(uploadUri, )
            client.Headers.Add("Authorization", "Token " + key);
            client.Headers.Add("Content-Type", "application/json");
            //var response = client.UploadString(uploadUri, e.Update.ToJson());
            //Console.WriteLine("params: {0}, {1}", e.Update.ToJson(), response);
        }

        static void CheckSettings()
        {
            if (Properties.Settings.Default.id == "" || Properties.Settings.Default.key == "")
                PromptSettings();
            else
            {
                Console.Write("Using existing settings, press a key to enter new settings");
                DateTime start = DateTime.Now;
                while ((DateTime.Now - start).Seconds < 5 && !Console.KeyAvailable)
                {
                    Console.Write(".");
                    Thread.Sleep(1000);
                }
                Console.WriteLine();

                if (Console.KeyAvailable)
                {
                    Console.ReadKey(); // eat up the entered key
                    Console.Clear();
                    PromptSettings();
                }
            }
        }

        static void PromptSettings()
        {
            Console.Write("Enter your id: ");
            Properties.Settings.Default.id = Console.ReadLine();
            Console.Write("Enter your private key: ");
            Properties.Settings.Default.key = Console.ReadLine();
            Properties.Settings.Default.Save();
        }

        static bool IsUpToDate()
        {
            Uri apiUri = new Uri(SITE_URL + "api/version");
            WebClient client = new WebClient();
            string response;
            try
            {
                response = client.DownloadString(apiUri);
            } catch (Exception)
            {
                return false;
            }

            int siteApiVersion = 0;
            int.TryParse(response, out siteApiVersion);

            if (siteApiVersion > API_VERSION)
                return false;

            return true;
        }
    }
}
