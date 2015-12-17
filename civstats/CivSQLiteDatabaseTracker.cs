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
        protected readonly string DatabaseName;

        public event EventHandler<StatsTrackerEventArgs> Changed;

        public CivSQLiteDatabaseTracker(string databaseName)
        {
            DatabaseName = databaseName;
            watcher = new CivFileWatcher(DatabaseName, "db", ReadDatabase);
        }

        private void ReadDatabase(string fullpath)
        {
            // This code was preventing Modding.DeleteUserData from working because 
            // CivStats' sqlite lib (System.Data.SQLite) would not release the database file
            // See http://stackoverflow.com/questions/8511901/system-data-sqlite-close-not-releasing-database-file
            // for the solution thats implemented
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            using (SQLiteConnection dbConnection = new SQLiteConnection("Data Source=" + fullpath + ";Version=3;"))
            {
                try
                {
                    dbConnection.Open();

                    SQLiteDataReader reader;
                    using (SQLiteCommand nameQuery = new SQLiteCommand("SELECT * FROM SimpleValues", dbConnection))
                    {
                        reader = nameQuery.ExecuteReader();
                    }

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string key = reader["name"].ToString();
                            string val = reader["value"].ToString();
                            pairs[key] = val;
                        }
                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error reading {0} database: {1}", DatabaseName, e.Message);
                }
                finally
                {
                    dbConnection.Close();
                }
            }

            if (pairs.Count != 0) // skip if empty db
            {
                ParseDatabaseEntries(pairs);
                // raise the event
                EmitEvent(new StatsTrackerEventArgs());
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
