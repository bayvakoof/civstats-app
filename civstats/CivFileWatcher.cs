using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace civstats
{
    public class CivFileWatcher
    {
        public delegate void ProcessFileChangeDelegate(string fullpath);

        private string filename;
        private string filetype;
        private ProcessFileChangeDelegate callback;

        private FileSystemWatcher watcher;
        private const string gameDirectory = "\\Documents\\My Games\\Sid Meier's Civilization 5";
        
        public CivFileWatcher(string filename, string filetype, ProcessFileChangeDelegate callback)
        {
            this.filename = filename;
            this.filetype = filetype;
            this.callback = callback;

            watcher = new FileSystemWatcher();
            watcher.Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + gameDirectory;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*." + filetype;
            watcher.IncludeSubdirectories = true;
            watcher.Changed += new FileSystemEventHandler(ChangeHandler);
            watcher.EnableRaisingEvents = true;
        }

        private void ChangeHandler(object source, FileSystemEventArgs e)
        {
            if (e.Name.IndexOf(filename) != -1)
            {
                callback.DynamicInvoke(e.FullPath);
                return;
            }
        }
    }
}
