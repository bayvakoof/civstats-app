using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Threading;

namespace civstats
{
    public abstract class CivSQLiteStatsTracker : IStatsTracker
    {
        protected CivFileWatcher watcher;
        protected SQLiteConnection dbConnection;
        protected DateTime lastUpdated;

        public event EventHandler<StatsTrackerEventArgs> Changed;

        public CivSQLiteStatsTracker(string dbname)
        {
            dbConnection = null;
            lastUpdated = DateTime.Now;
            watcher = new CivFileWatcher(dbname, "db", HandleDBUpdate);
        }

        private void HandleDBUpdate(string fullpath)
        {
            /* every ModUserData.SetValue writes to the file immediately (and fires off the event),
               so wait 1 second after the first SetValue to ensure that all values that
               were to be updated have been updated */
            Thread.Sleep(1000); 
            if (lastUpdated != null && DateTime.Now.Subtract(lastUpdated).TotalSeconds < 5)
                return; // don't send updates too frequently

            if (dbConnection == null)
            {
                dbConnection = new SQLiteConnection("Data Source=" + fullpath + ";Version=3;");
            }

            try
            {
                dbConnection.Open();
                SQLiteCommand nameQuery = new SQLiteCommand("SELECT * FROM SimpleValues", dbConnection);
                SQLiteDataReader reader = nameQuery.ExecuteReader();

                Dictionary<string, string> pairs = new Dictionary<string, string>();
                while (reader.Read())
                {
                    string key = reader["name"].ToString();
                    string value = reader["value"].ToString();
                    pairs[key] = value;
                }

                // raise the event
                StatsUpdate update = MakeStatsUpdate(pairs);
                EmitEvent(new StatsTrackerEventArgs(update));
                lastUpdated = DateTime.Now;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        private void EmitEvent(StatsTrackerEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<StatsTrackerEventArgs> handler = Changed;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public abstract StatsUpdate MakeStatsUpdate(Dictionary<string, string> pairs);
    }
}
