﻿using System;
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
        const string SiteUrl = "http://localhost:3000/";
        static Dictionary<Type, Uri> TrackerUriMap;
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
                new WarEventsTracker(),
                new NaturalWondersTracker()
            };

            string baseUrl = SiteUrl + "players/" + id;
            TrackerUriMap = new Dictionary<Type, Uri>
            {
                { typeof(GameTracker), new Uri(baseUrl + "/games") },
                { typeof(DemographicsTracker), new Uri(baseUrl + "/demographics") },
                { typeof(PolicyChoicesTracker), new Uri(baseUrl + "/policies") },
                { typeof(ReligionTracker), new Uri(baseUrl + "/religions") },
                { typeof(WondersTracker), new Uri(baseUrl + "/wonders") },
                { typeof(WarEventsTracker), new Uri(baseUrl + "/wars") },
                { typeof(NaturalWondersTracker), new Uri(baseUrl + "/wonders") },
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
            WebClient client = new WebClient();
            client.Headers.Add("Authorization", "Token " + key);
            client.Headers.Add("Content-Type", "application/json");
            
            try
            {
                var response = client.UploadString(TrackerUriMap[source.GetType()], serializer.Serialize(source));
                Console.WriteLine(response);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: {0}, params: {1}", exception.Message, serializer.Serialize(source));
            }
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
