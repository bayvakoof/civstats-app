using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace civstats
{
    class Program
    {
        static string id;
        static string key;

        static void Main(string[] args)
        {
            CheckSettings();
            id = Properties.Settings.Default.id;
            key = Properties.Settings.Default.key;

            DemographicsTracker dt = new DemographicsTracker();
            PoliciesTracker pt = new PoliciesTracker();
            ReligionTracker rt = new ReligionTracker();

            dt.Changed += StatsTrackerHandler;
            pt.Changed += StatsTrackerHandler;
            rt.Changed += StatsTrackerHandler;

            Console.WriteLine("Reporting civ stats... press any key to exit when you're done playing");
            Console.ReadKey();
        }
        
        static void StatsTrackerHandler(object source, StatsTrackerEventArgs e)
        {
            Uri siteUri = new Uri("http://civstats-byvkf.rhcloud.com/players/" + id + "/upload");
            WebClient client = new WebClient();
            client.Headers.Add("Authorization", "Token " + key);
            client.Headers.Add("Content-Type", "application/json");
            var response = client.UploadString(siteUri, e.Update.ToJson());
            Console.WriteLine(response);
        }

        static void CheckSettings()
        {
            if (Properties.Settings.Default.id == "" || Properties.Settings.Default.key == "")
                PromptSettings();
            else
            {
                Nullable<bool> useExisting = null;
                Console.Write("Use existing settings? (y/n) ");
                do
                {
                    ConsoleKeyInfo response = Console.ReadKey();
                    switch (response.KeyChar)
                    {
                        case 'y':
                        case 'Y':
                            useExisting = true;
                            break;
                        case 'n':
                        case 'N':
                            useExisting = false;
                            break;
                    }
                } while (useExisting == null);
                Console.WriteLine("");

                if (useExisting == false)
                    PromptSettings();
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
    }
}
