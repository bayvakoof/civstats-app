﻿using System;
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

        public event EventHandler<StatsTrackerEventArgs> Changed;

        public CivSQLiteStatsTracker(string dbname)
        {
            dbConnection = null;
            watcher = new CivFileWatcher(dbname, "db", HandleDBUpdate);
        }

        private void HandleDBUpdate(string fullpath)
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
                    string value = reader["value"].ToString();
                    pairs[key] = value;
                }

                // raise the event
                StatsUpdate update = MakeStatsUpdate(pairs);
                EmitEvent(new StatsTrackerEventArgs(update));
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
