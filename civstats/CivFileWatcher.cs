using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    class CivFileWatcher
    {
        public delegate void ProcessChangeDelegate(string fullpath);
        private Dictionary<string, ProcessChangeDelegate> watches;
        private FileSystemWatcher watcher;
        private const string gameDirectory = "\\Documents\\My Games\\Sid Meier's Civilization 5";
        
        public CivFileWatcher()
        {
            watches = new Dictionary<string, ProcessChangeDelegate>();

            watcher = new FileSystemWatcher();
            watcher.Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + gameDirectory;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.db";
            watcher.IncludeSubdirectories = true;
            watcher.Changed += new FileSystemEventHandler(ChangeHandler);
            watcher.EnableRaisingEvents = true;
        }

        private void ChangeHandler(object source, FileSystemEventArgs e)
        {
            foreach (var key in watches.Keys)
            {
                if (e.Name.IndexOf(key) != -1)
                {
                    watches[key].DynamicInvoke(e.FullPath);
                    return;
                }
            }
        }

        public void AddWatch(string filename, ProcessChangeDelegate callback)
        {
            watches.Add(filename, callback);
        }
    }
}
