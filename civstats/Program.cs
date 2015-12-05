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
        static Serializer serializer;
        const string SiteUrl = "http://civstats-byvkf.rhcloud.com/";
#if DEBUG
        const int ApiVersion = int.MaxValue;
#else
        const int ApiVersion = 1; // the version of the API the app is compatible with
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
            serializer = new Serializer();

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
            Uri uploadUri = new Uri(SiteUrl + "players/" + id + "/update");
            WebClient client = new WebClient();
            client.Headers.Add("Authorization", "Token " + key);
            client.Headers.Add("Content-Type", "application/json");
            var response = client.UploadString(uploadUri, serializer.Serialize(source));
            Console.WriteLine("params: {0}, {1}", serializer.Serialize(source), response);
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
            Uri apiUri = new Uri(SiteUrl + "api/version");
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

            if (siteApiVersion > ApiVersion)
                return false;

            return true;
        }
    }
}
