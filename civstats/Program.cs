using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace civstats
{
    class Program
    {
        static Uri siteUri;
        static WebClient client;

        static void Main(string[] args)
        {
            siteUri = new Uri("http://civstats-byvkf.rhcloud.com/update");
            client = new WebClient();

            DemographicsTracker dt = new DemographicsTracker();
            PoliciesTracker pt = new PoliciesTracker();
            ReligionTracker rt = new ReligionTracker();

            dt.Changed += StatsTrackerHandler;
            pt.Changed += StatsTrackerHandler;
            rt.Changed += StatsTrackerHandler;

            Console.WriteLine("Reporting civ stats... press any key to exit");
            Console.ReadKey();
        }
        
        static void StatsTrackerHandler(object source, StatsTrackerEventArgs e)
        {
            client.UploadStringAsync(siteUri, e.Update.ToJson());
        }
    }
}
