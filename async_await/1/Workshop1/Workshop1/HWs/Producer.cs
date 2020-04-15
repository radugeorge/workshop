using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Workshop1.HWs
{
    class Producer
    {
        private static FileSystemWatcher watcher;

        public static void Init(BlockingCollection<string> fileQueue)
        {
            watcher = new FileSystemWatcher
            {
                Path = @"..\..\..\..\..\..\filesToBeProcessed",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.LastWrite,
                Filter = "*.*"
            };
            watcher.Created += new FileSystemEventHandler((sender, e) => OnCreated(fileQueue, sender, e));
            watcher.EnableRaisingEvents = true;
        }

        private static void OnCreated(BlockingCollection<string> fileQueue, object sender, FileSystemEventArgs e)
        {
            fileQueue.Add(e.FullPath);
        }

        internal static void Deinit()
        {
            watcher.Dispose();
        }
    }
}
