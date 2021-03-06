﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive;

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
            watcher.EnableRaisingEvents = true;

            IObservable<EventPattern<FileSystemEventArgs>> watcherObserver = Observable.FromEventPattern<FileSystemEventArgs>(watcher, "Changed");
            watcherObserver.Where(x => x.EventArgs.Name.Contains(filename))
                .Throttle(TimeSpan.FromSeconds(2))
                .Subscribe(events => ChangeHandler(events.Sender, events.EventArgs));
        }

        ~CivFileWatcher()
        {
            watcher.Dispose();
        }

        private void ChangeHandler(object source, FileSystemEventArgs e)
        {
            callback.DynamicInvoke(e.FullPath);
        }
    }
}
