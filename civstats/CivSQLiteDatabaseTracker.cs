using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Threading;

namespace civstats
{
    /**
    Tracks changes to the civstats databases
    */
    public abstract class CivSQLiteDatabaseTracker : IStatsTracker
    {
        protected CivFileWatcher watcher;
        protected SQLiteConnection dbConnection;

        public event EventHandler<StatsTrackerEventArgs> Changed;

        public CivSQLiteDatabaseTracker(string dbname)
        {
            dbConnection = null;
            watcher = new CivFileWatcher(dbname, "db", ReadDatabase);
        }

        private void ReadDatabase(string fullpath)
        {
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
                    string val = reader["value"].ToString();
                    pairs[key] = val;
#if DEBUG
                    Console.WriteLine("Got SimpleValue update: {0}, {1}", key, val);
#endif
                }

                ParseDatabaseEntries(pairs);
                // raise the event
                EmitEvent(new StatsTrackerEventArgs());
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

        protected abstract void ParseDatabaseEntries(Dictionary<string, string> pairs);
    }
}
